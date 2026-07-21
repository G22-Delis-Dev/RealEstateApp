using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Offers;

namespace RealEstateApp.WebApp.Controllers;

[Authorize(Roles = "Client")]
public class ClientController : BaseController
{
    private readonly IPropertyService _propertyService;
    private readonly IFavoriteService _favoriteService;
    private readonly IMessageService _messageService;
    private readonly IOfferService _offerService;

    public ClientController(
        IPropertyService propertyService,
        IFavoriteService favoriteService,
        IMessageService messageService,
        IOfferService offerService)
    {
        _propertyService = propertyService;
        _favoriteService = favoriteService;
        _messageService = messageService;
        _offerService = offerService;
    }

    // GET: /Client
    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetAvailableAsync();
        return View(properties);
    }

    // GET: /Client/Favorites
    public async Task<IActionResult> Favorites()
    {
        var favorites = await _favoriteService.GetByClientAsync(CurrentUserId);

        if (!favorites.Any())
            TempData["InfoMessage"] = "No tiene propiedades favoritas disponibles en este momento.";

        return View(favorites);
    }

    // POST: /Client/ToggleFavorite/5
    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int propertyId)
    {
        await _favoriteService.ToggleAsync(CurrentUserId, propertyId);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Client/PropertyDetail/5
    public async Task<IActionResult> PropertyDetail(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null)
        {
            TempData["ErrorMessage"] = "La propiedad solicitada no existe o no se encuentra disponible.";
            return RedirectToAction(nameof(Index));
        }

        var messages = await _messageService.GetConversationAsync(id, CurrentUserId, property.AgentId);
        var offers = await _offerService.GetByClientAndPropertyAsync(CurrentUserId, id);

        ViewBag.Messages = messages;
        ViewBag.Offers = offers;
        ViewBag.CanOffer = property.Status == "Disponible" && !offers.Any(o => o.Status == "Pendiente");

        return View(property);
    }

    // POST: /Client/SendMessage
    [HttpPost]
    public async Task<IActionResult> SendMessage(int propertyId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            TempData["ErrorMessage"] = "Debe escribir un mensaje antes de enviarlo.";
            return RedirectToAction(nameof(PropertyDetail), new { id = propertyId });
        }

        var property = await _propertyService.GetByIdAsync(propertyId)
            ?? throw new NotFoundException(nameof(RealEstateApp.Domain.Entities.Property), propertyId);

        await _messageService.SendAsync(propertyId, CurrentUserId, property.AgentId, senderId: CurrentUserId, content);

        return RedirectToAction(nameof(PropertyDetail), new { id = propertyId });
    }

    // POST: /Client/SendOffer
    [HttpPost]
    public async Task<IActionResult> SendOffer(CreateOfferViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Debe ingresar el monto de la oferta.";
            return RedirectToAction(nameof(PropertyDetail), new { id = model.PropertyId });
        }

        await _offerService.CreateAsync(model, CurrentUserId);
        TempData["SuccessMessage"] = "Su oferta fue enviada correctamente.";

        return RedirectToAction(nameof(PropertyDetail), new { id = model.PropertyId });
    }
}