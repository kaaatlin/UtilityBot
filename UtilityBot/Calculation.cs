using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace UtilityBot
{
    public class Calculation
    {
        public int Start(string userChoiseType, Message message)
        {
            int sumary = 0;
            if (userChoiseType == "symb")
            {
                sumary = message.Text.Length;

            }
            else if (userChoiseType == "sum")
            {
                string num = Convert.ToString(message.Text);
                var words = num.Split();
                for (int i = 0; i < words.Length; sumary += int.Parse(words[i++])) ;
            }
            return sumary;
        }
    }
}
