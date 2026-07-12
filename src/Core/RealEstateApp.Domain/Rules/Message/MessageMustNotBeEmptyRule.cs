namespace RealEstateApp.Domain.Rules.Message;

public sealed class MessageMustNotBeEmptyRule : IBusinessRule
{
    private readonly string _content;
    public MessageMustNotBeEmptyRule(string content) => _content = content;

    public bool IsBroken() => string.IsNullOrWhiteSpace(_content);
    public string Message => "Debe escribir un mensaje antes de enviarlo.";
}