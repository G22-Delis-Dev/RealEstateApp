using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.WebApp.Controllers;

[Authorize(Roles = "Agente")]
public class AgentPropertiesController : BaseController
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly ISaleTypeService _saleTypeService;
    private readonly IImprovementService _improvementService;
    private readonly IOfferService _offerService;
    private readonly IMessageService _messageService;

    public AgentPropertiesController(
        IPropertyService propertyService,
        IPropertyTypeService propertyTypeService,
        ISaleTypeService saleTypeService,
        IImprovementService improvementService,
        IOfferService offerService,
        IMessageService messageService)
    {
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
        _saleTypeService = saleTypeService;
        _improvementService = improvementService;
        _offerService = offerService;
        _messageService = messageService;
    }

    // GET: /AgentProperties
    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetByAgentIdAsync(CurrentUserId);

        if (!properties.Any())
            TempData["InfoMessage"] = "No tiene propiedades disponibles registradas en este momento.";

        return View(properties);
    }

    // GET: /AgentProperties/Create
    public async Task<IActionResult> Create()
    {
        var model = new CreatePropertyViewModel();
        await PopulateSelectsAsync(model);
        return View(model);
    }

    // POST: /AgentProperties/Create
    [HttpPost]
    public async Task<IActionResult> Create(CreatePropertyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectsAsync(model);
            return View(model);
        }

        try
        {
            await _propertyService.CreateAsync(model, CurrentUserId);
            return RedirectWithSuccess(nameof(Index), "La propiedad fue creada correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectsAsync(model);
            return View(model);
        }
    }

    // GET: /AgentProperties/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.AgentId != CurrentUserId)
        {
            TempData["ErrorMessage"] = "No tiene permisos para modificar esta propiedad.";
            return RedirectToAction(nameof(Index));
        }

        if (property.Status != "Disponible")
        {
            TempData["ErrorMessage"] = "No se puede modificar una propiedad que ya fue vendida.";
            return RedirectToAction(nameof(Index));
        }

        var model = new EditPropertyViewModel
        {
            Id = property.Id,
            Code = property.Code,
            PropertyTypeId = property.PropertyTypeId,
            SaleTypeId = property.SaleTypeId,
            Price = property.Price,
            Description = property.Description,
            Size = property.Size,
            Rooms = property.Rooms,
            Bathrooms = property.Bathrooms,
            ImprovementIds = property.ImprovementIds,
            CurrentImageUrls = property.ImageUrls
        };

        await PopulateSelectsAsync(model);
        return View(model);
    }

    // POST: /AgentProperties/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditPropertyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectsAsync(model);
            return View(model);
        }

        try
        {
            await _propertyService.UpdateAsync(id, model, CurrentUserId);
            return RedirectWithSuccess(nameof(Index), "La propiedad fue actualizada correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectsAsync(model);
            return View(model);
        }
    }

    // GET: /AgentProperties/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.AgentId != CurrentUserId)
        {
            TempData["ErrorMessage"] = "No tiene permisos para eliminar esta propiedad.";
            return RedirectToAction(nameof(Index));
        }

        return View(property);
    }

    // POST: /AgentProperties/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _propertyService.DeleteAsync(id, CurrentUserId);
            return RedirectWithSuccess(nameof(Index), "La propiedad fue eliminada correctamente.");
        }
        catch (BusinessRuleValidationException ex)
        {
            return RedirectWithError(nameof(Index), ex.Message);
        }
    }

    // GET: /AgentProperties/Conversations/5
    public async Task<IActionResult> Conversations(int propertyId)
    {
        var clientIds = await _messageService.GetClientIdsWithConversationAsync(propertyId);
        ViewBag.PropertyId = propertyId;
        return View(clientIds);
    }

    // GET: /AgentProperties/Conversation/5?clientId=xxx
    public async Task<IActionResult> Conversation(int propertyId, string clientId)
    {
        var messages = await _messageService.GetConversationAsync(propertyId, clientId, CurrentUserId);
        ViewBag.PropertyId = propertyId;
        ViewBag.ClientId = clientId;
        return View(messages);
    }

    // POST: /AgentProperties/Reply
    [HttpPost]
    public async Task<IActionResult> Reply(int propertyId, string clientId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            TempData["ErrorMessage"] = "Debe escribir un mensaje antes de enviarlo.";
            return RedirectToAction(nameof(Conversation), new { propertyId, clientId });
        }

        await _messageService.SendAsync(propertyId, clientId, CurrentUserId, senderId: CurrentUserId, content);
        return RedirectToAction(nameof(Conversation), new { propertyId, clientId });
    }

    // GET: /AgentProperties/Offers/5
    public async Task<IActionResult> Offers(int propertyId)
    {
        var offers = await _offerService.GetByPropertyAsync(propertyId);
        ViewBag.PropertyId = propertyId;
        return View(offers);
    }

    // POST: /AgentProperties/AcceptOffer
    [HttpPost]
    public async Task<IActionResult> AcceptOffer(int offerId, int propertyId)
    {
        try
        {
            await _offerService.AcceptAsync(offerId, CurrentUserId);
            TempData["SuccessMessage"] = "La oferta fue aceptada correctamente y la propiedad fue marcada como vendida.";
        }
        catch (BusinessRuleValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Offers), new { propertyId });
    }

    // POST: /AgentProperties/RejectOffer
    [HttpPost]
    public async Task<IActionResult> RejectOffer(int offerId, int propertyId)
    {
        try
        {
            await _offerService.RejectAsync(offerId, CurrentUserId);
            TempData["SuccessMessage"] = "La oferta fue rechazada correctamente.";
        }
        catch (BusinessRuleValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Offers), new { propertyId });
    }

    private async Task PopulateSelectsAsync(CreatePropertyViewModel model)
    {
        model.AvailablePropertyTypes = (await _propertyTypeService.GetAllAsync())
            .Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
        model.AvailableSaleTypes = (await _saleTypeService.GetAllAsync())
            .Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
        model.AvailableImprovements = (await _improvementService.GetAllAsync())
            .Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
    }

    private async Task PopulateSelectsAsync(EditPropertyViewModel model)
    {
        model.AvailablePropertyTypes = (await _propertyTypeService.GetAllAsync())
            .Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
        model.AvailableSaleTypes = (await _saleTypeService.GetAllAsync())
            .Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
        model.AvailableImprovements = (await _improvementService.GetAllAsync())
            .Select(t => new SelectOption { Id = t.Id, Name = t.Name }).ToList();
    }
}