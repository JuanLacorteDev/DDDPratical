using DDDPratical.Domain.Commom.Base;
using DDDPratical.Domain.Commom.Enums;
using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Commom.Validations;
using DDDPratical.Domain.Events.Order;
using DDDPratical.Domain.ValueObjects;

namespace DDDPratical.Domain.Entities;

public sealed class Order : AggregateRoot
{
    public Guid ClientId { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;


    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();


    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();


    private Order(Guid clientId, DeliveryAddress deliveryAddress)
    {
        Guard.AgainstEmptyGuid(clientId, nameof(clientId));
        Guard.AgainstNull(deliveryAddress, nameof(deliveryAddress));

        ClientId = clientId;
        DeliveryAddress = deliveryAddress;
        Status = OrderStatus.Pending;
        TotalAmount = 0;

        GenerateOrderNumber();
    }

    public static Order CreateNewOrder(Guid clientId, DeliveryAddress deliveryAddress)
    {
        return new Order(clientId, deliveryAddress);
    }

    public void AddItem(OrderItem item)
    {
        Guard.AgainstNull(item, nameof(item));
        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Should only be able to add itens on pending orders");

        var existingItem = _items.Find(i => i.ProductId == item.ProductId);

        if (existingItem is not null)
            existingItem.AddQuantity(item.Quantity);
        else
            _items.Add(item);


        _items.Add(item);
        RecalculateTotalAmount();
        SetUpdatedAt();
    }

    public void RemoverItem(Guid itemId)
    {
        Guard.AgainstEmptyGuid(itemId, nameof(itemId));
        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Should only be able to remove itens on pending orders");

        var item = _items.Find(i => i.Id == itemId);
        Guard.AgainstNull(item, "Item not found in order");

        _items.Remove(item!);

        Guard.Against<DomainException>(
            _items.Count() == 0,
            "An order must have at least one item");

        RecalculateTotalAmount();
        SetUpdatedAt();
    }

    public void UpdateDeliveryAddress(DeliveryAddress newAddress)
    {
        Guard.AgainstNull(newAddress, nameof(newAddress));
        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Should only be able to change delivery address on pending orders");

        DeliveryAddress = newAddress;
        SetUpdatedAt();
    }

    public Payment InitPayment(PaymentMethod method)
    {
        Guard.Against<DomainException>(
            Items.Count() == 0,
            "Should only be able to init payment in a order with items");

        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Should only be able to init payment on pending orders");

        Guard.Against<DomainException>(
            _payments.Any(p => p.Status == PaymentStatus.Pending),
            "There is already a pending payment for this order");

        var payment = new Payment(Id, method, TotalAmount);

        _payments.Add(payment);
        SetUpdatedAt();

        return payment;
    }

    public void HandlePaymentSuccess(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment is null)
            return;

        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Should only be able to process payment on pending orders");

        Guard.Against<DomainException>(
            payment!.Status != PaymentStatus.Pending,
            "Only pending payments can be marked as successful");


        payment!.MarkAsPaid();
        Status = OrderStatus.Paid;

        SetUpdatedAt();
    }


    public void HandlePaymentFailure(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment is null)
            return;

        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Should only be able to process payment on pending orders");

        Guard.Against<DomainException>(
            payment!.Status != PaymentStatus.Pending,
            "Only pending payments can be marked as failed");

        payment!.MarkAsRefused();

        Status = OrderStatus.Canceled;
        SetUpdatedAt();

        AddDomainEvent(new OrderCancelledEvent(Id, ClientId, Status, ReasonCancellation.PaymentIssue, paymentId));
    }

    public void SetAsSeparation()
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.Paid,
            "Only paid orders can request for separation");

        Status = OrderStatus.InSeparation;
        SetUpdatedAt();
    }

    public void SetAsShipped()
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.InSeparation,
            "Only orders in separation can be shipped");

        Status = OrderStatus.Shipped;
        SetUpdatedAt();

        AddDomainEvent(new OrderSentEvent(Id, ClientId, DeliveryAddress));
    }

    public void SetAsDelivered()
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.Shipped,
            "Only shipped orders can be delivered");

        Status = OrderStatus.Delivered;
        SetUpdatedAt();

        AddDomainEvent(new OrderDeliveredEvent(Id, ClientId));
    }

    public void CancelOrder(ReasonCancellation reason)
    {
        Guard.Against<DomainException>(
            Status >= OrderStatus.InSeparation,
            "Is not possible to cancel once in separation");

        Status = OrderStatus.Canceled;

        SetUpdatedAt();
        AddDomainEvent(new OrderCancelledEvent(Id, ClientId, Status, reason ?? ReasonCancellation.Other, _payments.LastOrDefault()?.Id));
    }


    private void RecalculateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.TotalAmount);
    }

    public void GenerateOrderNumber()
    {
        OrderNumber = $"ORD-{Id.ToString()[..8].ToUpper()}";
    }


}
