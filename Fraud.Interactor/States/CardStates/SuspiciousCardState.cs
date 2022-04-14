﻿using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Interactor.MessageBroking;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;
using Newtonsoft.Json;

namespace Fraud.Interactor.States.CardStates
{
    public class SuspiciousCardState : ICardState
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.Suspicious;
        
        public SuspiciousCardState(Card card)
        {
            Card = card;
        }
        
        public async Task HandleState()
        {
            var cardStateMessage = JsonConvert.SerializeObject(this.Card);
            // TODO: Change implementation instance to interface instance
            IMessageBrokerService messageBrokerService = new RabbitMqMessageBrokerService(new RabbitMqConfigurations());
            messageBrokerService
                .Send(routingKey: "some-routing-key", exchangeName: "some-ex-name", message: cardStateMessage);        }
    }
}