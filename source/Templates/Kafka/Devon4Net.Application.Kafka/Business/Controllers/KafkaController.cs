using Confluent.Kafka;
using Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers;
using Devon4Net.Infrastructure.Kafka.Handlers;
using Devon4Net.Infrastructure.Logger.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.Kafka.Business.Controllers
{
    /// <summary>
    /// KafkaController sample
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class KafkaController : ControllerBase
    {
        private MessageProducerHandler MessageProducer { get; }
        private MessageProducerHandler2 MessageProducer2 { get; }
        private IKafkaHandler KafkaHandler { get; }

        /// <summary>
        /// KafkaController constructor
        /// </summary>
        /// <param name="messageProducer"></param>
        /// <param name="messageProducer2"></param>
        /// <param name="kafkaHandler"></param>
        public KafkaController(MessageProducerHandler messageProducer, MessageProducerHandler2 messageProducer2, IKafkaHandler kafkaHandler)
        {
            MessageProducer = messageProducer;
            MessageProducer2 = messageProducer2;
            KafkaHandler = kafkaHandler;
        }

        /// <summary>
        /// Delivers a Kafka message
        /// </summary>
        /// <param name="key">message key</param>
        /// <param name="value">message value</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DeliveryResult<string, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/deliver")]
        public async Task<IActionResult> DeliverMessage(string key, string value)
        {
            Devon4NetLogger.Debug("Executing DeliverMessage from controller KafkaController");
            var result = await MessageProducer.SendMessage(key, value).ConfigureAwait(false);
            return Ok(result);
        }


        /// <summary>
        /// Delivers a Kafka message
        /// </summary>
        /// <param name="key">message key</param>
        /// <param name="value">message value</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DeliveryResult<string, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/deliver2")]
        public async Task<IActionResult> DeliverMessage2(string key, string value)
        {
            Devon4NetLogger.Debug("Executing DeliverMessage from controller KafkaController");
            var result = await MessageProducer2.SendMessage(key, value).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// Delivers a Kafka message
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/topic")]
        public async Task<IActionResult> CreateTopic(string topicName)
        {
            Devon4NetLogger.Debug("Executing CreateTopic from controller KafkaController");
            return Ok(await KafkaHandler.CreateTopic("Admin1", topicName).ConfigureAwait(false));
        }

        /// <summary>
        /// Delivers a Kafka message
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/topic")]
        public async Task<IActionResult> DeleteTopicMessage(string topicName)
        {
            Devon4NetLogger.Debug("Executing DeleteTopicMessage from controller KafkaController");
            return Ok(await KafkaHandler.DeleteTopic("Admin1", new List<string> { topicName }).ConfigureAwait(false));
        }
    }
}
