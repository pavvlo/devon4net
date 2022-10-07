﻿using Confluent.Kafka;
using System.Text.Json;

namespace Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Dto
{
    public class DataPieceDto<T> where T : class
    {
        public Guid Guid { get; set; }
        public string FileName { get; set; }
        public int TotalParts { get; set; }
        public string FileExtension { get; set; }
        public int PieceOffset { get; set; }
        public int Position { get; set; }
        public T Data { get; set; }

    }
}
