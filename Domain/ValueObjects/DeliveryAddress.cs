using DDDPratical.Domain.Commom.Base;
using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Commom.Validations;
using System.Text.RegularExpressions;

namespace DDDPratical.Domain.ValueObjects;

public class DeliveryAddress : ValueObject
{
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }

    private DeliveryAddress(
        string postalCode, string street, string number, string complement,
        string neighborhood, string state, string city, string country
        )
    {
        Guard.AgainstNullOrWhiteSpace(postalCode, nameof(postalCode));
        Guard.AgainstNullOrWhiteSpace(street, nameof(street));
        Guard.AgainstNullOrWhiteSpace(number, nameof(number));
        Guard.AgainstNullOrWhiteSpace(neighborhood, nameof(neighborhood));
        Guard.AgainstNullOrWhiteSpace(state, nameof(state));
        Guard.AgainstNullOrWhiteSpace(city, nameof(city));
        Guard.AgainstNullOrWhiteSpace(country, nameof(country));

        if (!Regex.IsMatch(postalCode, @"^\d{5}-\d{3}$"))
            throw new DomainException("Invalid postal code format. Expected format is '12345-678'.");

        PostalCode = postalCode;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        State = state;
        City = city;
        Country = country;
    }

    public static DeliveryAddress Create(
        string postalCode, string street, string number, string complement,
        string neighborhood, string state, string city, string country
        )
    {
        return new DeliveryAddress(
            postalCode, street, number, complement,
            neighborhood, state, city, country
            );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PostalCode;
        yield return Street;
        yield return Number;
        yield return Complement ?? string.Empty;
        yield return Neighborhood;
        yield return State;
        yield return City;
        yield return Country;
    }


}
