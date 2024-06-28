using App.Common.Data;

namespace App.Catalog.Interfaces.Views
{
    public interface ICatalogCardView
    {
        void Setup(string cardNumber, string name, int level, int Hp, string costType, string rareLevel);
        void Unspawn();
    }
}