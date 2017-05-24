using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole.Models
{
    public class TranslateResponse
    {
        public TranslateContent content { get; set; }
        public int status { get; set; }

    }
    [Serializable]
    public class TranslateContent
    {
        public string ph_am { get; set; }
        public string ph_am_mp3        { get; set; }
        public string ph_en { get; set; }
        public string ph_en_mp3 { get; set; }
        public string ph_tts_mp3 { get; set; }
        public List<string> word_mean { get; set; }

        [DataMember]
        public string s_out { get; set; }
    }
}
