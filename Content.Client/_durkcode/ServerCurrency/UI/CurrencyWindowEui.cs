using Content.Client.Eui;
using Content.Shared._durkcode.ServerCurrency;
using Content.Shared._durkcode.ServerCurrency.UI;
using Robust.Shared.Prototypes;

namespace Content.Client._durkcode.ServerCurrency.UI
{
    public class CurrencyEui : BaseEui
    {
        private readonly CurrencyWindow _window;
        public CurrencyEui()
        {
            _window = new CurrencyWindow();
            _window.OnClose += () => SendMessage(new CurrencyEuiMsg.Close());
            _window.OnBuy += OnBuyMsg;
        }

        private void OnBuyMsg(ProtoId<TokenListingPrototype> tokenId)
        {
            SendMessage(new CurrencyEuiMsg.Buy
            {
                TokenId = tokenId
            });
            SendMessage(new CurrencyEuiMsg.Close());
        }

        public override void Opened()
        {
            _window.OpenCentered();
        }
        public override void Closed()
        {
            _window.Close();
        }
    }
}
