using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlRestaurant.Integration.MessageBus;

public interface IMessageBus
{
    Task Publish(BaseMessage message, string topicName);
}