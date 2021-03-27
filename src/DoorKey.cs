using System;
using System.Web.Script.Serialization;

namespace App
{
    public class DoorKey
    {
        public const int StatusNew = 0;
        public const int StatusSuccess = 1;
        public const int StatusFail = -1;

        public int? EncoderAnswer { get; set; }
        public string? EncoderAnswerText { get; set; }
        public int Status { get; set; }
        public string UserUuid { get; set; }
        public string Uuid { get; set; }
        public int SectorNumber { get; set; }
        public string CardType { get; set; }
        public string ExpiredAt { get; set; }
        public string CreatedAt { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public string[] Rooms { get; set; }
        public string CallbackUrl { get; set; }

        public DoorKey()
        {
            this.EncoderAnswer = null;
            this.EncoderAnswerText = null;
        }
    }
}
