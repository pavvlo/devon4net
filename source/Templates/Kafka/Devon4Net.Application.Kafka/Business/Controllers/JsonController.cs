﻿using Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers;
using Devon4Net.Application.Kafka.Business.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.Kafka.Business.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class JsonController : ControllerBase
    {
        public static readonly int NUM_CHAR = 5;
        public MessageProducerHandler Producer { get; set; }

        public JsonController(MessageProducerHandler messageProducer)
        {
            Producer = messageProducer;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/json")]
        public async Task<IActionResult> SplitJson(string json)
        {
            var trimmed = string.Concat(json.Where(c => !char.IsWhiteSpace(c)));
            string piece;
            double totalPieces = Math.Ceiling((double)trimmed.Length / NUM_CHAR);
            int pos = 0;
            while (trimmed.Length > 0)
            {
                piece = trimmed.Substring(0, trimmed.Length < NUM_CHAR ? trimmed.Length : NUM_CHAR);
                trimmed = trimmed.Substring(piece.Length);
                await Producer.SendMessage("json", piece
                    //new DataPiece<string>
                    //{
                    //    Data = piece, 
                    //    Position = pos,
                    //    TotalParts = (int) totalPieces
                    //}
                );
                pos++;
            }

            return Ok();
        }
    }
}
