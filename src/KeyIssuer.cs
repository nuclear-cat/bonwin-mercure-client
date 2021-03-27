using System;
using System.Diagnostics.Tracing;
using System.Text;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace App
{
    public class KeyIssuer
    {
        [DllImport("bwusbapi.dll")]
        private static extern int bw803_wrkey(int f_sec, byte[] crdinfo, byte[] rooms);
        [DllImport("bwusbapi.dll")]
        private static extern int bw803_rdkey(int f_sec, byte[] crdinfo, byte[] rooms);
        [DllImport("bwusbapi.dll")]
        private static extern int bw823_wrkey(int f_sec, byte[] crdinfo, byte[] rooms);
        [DllImport("bwusbapi.dll")]
        private static extern int bw823_rdkey(int f_sec, byte[] crdinfo, byte[] rooms);

        // Замок клиента BW893 (BW8838/8288/8088)
        [DllImport("bwusbapi.dll")]
        private static extern int bw893_wrkey(int f_sec, byte[] crdinfo, byte[] rooms);
        [DllImport("bwusbapi.dll")]
        private static extern int bw893_rdkey(int f_sec, byte[] crdinfo, byte[] rooms);

        [DllImport("bwusbapi.dll")]
        private static extern int bw8x5_wrkey(byte[] crdinfo, byte[] rooms);
        [DllImport("bwusbapi.dll")]
        private static extern int bw8x5_rdkey(byte[] crdinfo, byte[] rooms);

        [DllImport("bwusbapi.dll")]
        private static extern int dev_openbuzz(int f_sec);
        [DllImport("bwusbapi.dll")]
        private static extern int m1_reset();
        [DllImport("bwusbapi.dll")]
        private static extern int m1_halt();
        [DllImport("bwusbapi.dll")]
        private static extern int m1_hiselect(int req_code, byte[] csn);
        [DllImport("bwusbapi.dll")]
        private static extern int fm_reset(byte[] rlen, byte[] csn);
        [DllImport("bwusbapi.dll")]
        private static extern int oda_wrd_record(int wr_rd, byte[] dqhy, byte[] ymd, byte[] cinfo);

        App.CallbackSender callbackSender = new App.CallbackSender();

        public void issue(DoorKey doorKey)
        {
            try {
                int sectorNumber = doorKey.SectorNumber;
                string cardType = doorKey.CardType;
                string expiredAt = doorKey.ExpiredAt;
                string createdAt = doorKey.CreatedAt;
                string startPeriod = doorKey.StartPeriod;
                string endPeriod = doorKey.EndPeriod;
                int totalRooms = doorKey.Rooms.Length;
                string roomsInfo = (totalRooms + string.Join("", doorKey.Rooms)).PadRight(100, '0');
                string cardInfo = (cardType + expiredAt + startPeriod + endPeriod + createdAt).PadRight(100, '0');

                byte[] cardInfoBytes = System.Text.Encoding.GetEncoding("gb2312").GetBytes(cardInfo);
                byte[] roomsInfoBytes = System.Text.Encoding.GetEncoding("gb2312").GetBytes(roomsInfo);

                Console.WriteLine("Sector number: " + sectorNumber);
                Console.WriteLine("Card type: " + cardType);
                Console.WriteLine("Expired at: " + expiredAt);
                Console.WriteLine("Created at: " + expiredAt);
                Console.WriteLine("Start period: " + startPeriod);
                Console.WriteLine("End period: " + endPeriod);
                Console.WriteLine("Total rooms: " + totalRooms);
                Console.WriteLine("Rooms info: " + roomsInfo);
                Console.WriteLine("Card info: " + cardInfo);

                int encodingResult = bw893_wrkey(sectorNumber, cardInfoBytes, roomsInfoBytes);

                if (encodingResult == 0) {
                    Console.WriteLine("Encoding Success!");

                    doorKey.Status = DoorKey.StatusSuccess;
                    doorKey.EncoderAnswer = 0;
                    doorKey.EncoderAnswerText = null;

                } else {
                    Console.WriteLine("Encoding Error: " + encodingResult);

                    doorKey.Status = DoorKey.StatusFail;
                    doorKey.EncoderAnswer = encodingResult;
                    doorKey.EncoderAnswerText = null;
                }
            }
            catch (Exception err)
            {
                doorKey.Status = DoorKey.StatusFail;
                doorKey.EncoderAnswer = 1;
                doorKey.EncoderAnswerText = err.Message;

                Console.WriteLine("Encoding Error: " + err.ToString());
            }

            callbackSender.send(doorKey);
        }
    }
}