using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;
        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).ChoiseCode = callbackQuery.Data;

            // Генерим информационное сообщение
            string choiseText = callbackQuery.Data switch
            {
                "symb" => " Подсчет символов в строке",
                "sum" => " Подсчет суммы чисел",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбрано - {choiseText}.{Environment.NewLine}</b>", cancellationToken: ct, parseMode: ParseMode.Html);
            switch (callbackQuery?.Data)
            {
                case "sim":
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, "Напишите сообщение, чтобы мы подсчитали в нём количество символов!", cancellationToken: ct);
                    break;
                case "sum":
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, "Напишите числа через пробел и мы подсчитаем их сумму!", cancellationToken: ct);
                    break;

            }
        }
    }
}
