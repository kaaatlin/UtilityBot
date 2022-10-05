using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            string choiseType = _memoryStorage.GetSession(message.Chat.Id).ChoiseCode;
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Подсчет символов в строке" , $"symb"),
                        InlineKeyboardButton.WithCallbackData($" Подсчет суммы чисел" , $"sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Выберите, что хотите сделать.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно подсчитать символы в строке, или подсчитать сумму чисел.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
            }
            Calculation calculation = new Calculation();
            int sum = calculation.Start(choiseType, message);

            if (choiseType == "symb")
            {
                await _telegramClient.SendTextMessageAsync(message.From.Id, $"Длина сообщения: {sum} знаков", cancellationToken: ct);

            }
            else if (choiseType == "sum")
            {
                await _telegramClient.SendTextMessageAsync(message.From.Id, $"Сумма введённых чисел: {sum}", cancellationToken: ct);
            }
        }
    }
}
