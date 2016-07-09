using HandballCliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public static class GameShowController
    {
        public static void addFindCardItem(FindCardItem q)
        {
            if (!HandballMatch.getInstance().gameshowFindCardItems.Exists(element => element.id == q.id))
            {
                HandballMatch.getInstance().gameshowFindCardItems.Add(q);
            }
        }

        public static void updateFindCardItem(FindCardItem q)
        {
            if (HandballMatch.getInstance().gameshowFindCardItems.Exists(element => element.id == q.id))
            {
                HandballMatch.getInstance().gameshowFindCardItems.Find(element => element.id == q.id).id = q.id;
                HandballMatch.getInstance().gameshowFindCardItems.Find(element => element.id == q.id).type = q.type;
                HandballMatch.getInstance().gameshowFindCardItems.Find(element => element.id == q.id).picture = q.picture;
            }
        }

        public static void deleteGameshowFindCardItem(string id)
        {
            if (HandballMatch.getInstance().gameshowFindCardItems.Exists(element => element.id == int.Parse(id)))
            {
                HandballMatch.getInstance().gameshowFindCardItems.Remove(HandballMatch.getInstance().gameshowFindCardItems.Find(element => element.id == int.Parse(id)));
            }
        }

        public static void fillComboFindCardItemTypes(ComboBox combo)
        {
            combo.Items.Clear();

            combo.Items.Add(new KeyValuePair<string, int>(CGClientConstants.FindCardItemType.FRONT_CARD.ToString(), (int)CGClientConstants.FindCardItemType.FRONT_CARD));
            combo.Items.Add(new KeyValuePair<string, int>(CGClientConstants.FindCardItemType.LOOSER_CARD.ToString(), (int)CGClientConstants.FindCardItemType.LOOSER_CARD));
            combo.Items.Add(new KeyValuePair<string, int>(CGClientConstants.FindCardItemType.WINNER_CARD.ToString(), (int)CGClientConstants.FindCardItemType.WINNER_CARD));
        }

        public static String getFindCardItemPictureByType(CGClientConstants.FindCardItemType findCardItemType)
        {
            int type = (int)findCardItemType;
            FindCardItem aux = HandballMatch.getInstance().gameshowFindCardItems.Find(element => element.type == type);
            return aux.picture;
        }

        public static FindCardMatch getFindCardMatchById(long id)
        {
            FindCardMatch aux = HandballMatch.getInstance().gameshowFindCardMatches.Find(element => element.uid == id);
            return aux;
        }

        public static int getFindCardWinnerPositionById(long findCardMatchId)
        {
            Card posCard = getFindCardMatchById(findCardMatchId).Cards.Find(element => element.Win == true);
            return posCard.order;
        }

        public static void simulateFindCardMatches()
        {
            List<Card> cards = new List<Card>();
            Card aux = new Card();
            aux.order = 1;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 2;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 3;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 4;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 5;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 6;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 7;
            aux.Win = true;
            cards.Add(aux);
            aux = new Card();
            aux.order = 8;
            aux.Win = false;
            cards.Add(aux);
            aux = new Card();
            aux.order = 9;
            aux.Win = false;
            cards.Add(aux);

            FindCardMatch match = new FindCardMatch();
            match.Cards = cards;
            match.uid = 2454563456;
            match.dateTime = DateTime.Now;

            List<FindCardMatch> findcardsMatches = new List<FindCardMatch>();
            findcardsMatches.Add(match);

            HandballMatch.getInstance().gameshowFindCardMatches = findcardsMatches;
        }

        public static void simulateFindCardItems()
        {
            List<FindCardItem> items = new List<FindCardItem>();

            FindCardItem aux = new FindCardItem();
            aux.id = 1;
            aux.type = (int)CGClientConstants.FindCardItemType.FRONT_CARD;
            aux.picture = "\\Logos-Canal\\canal10cordoba";
            items.Add(aux);

            aux = new FindCardItem();
            aux.id = 2;
            aux.type = (int)CGClientConstants.FindCardItemType.LOOSER_CARD;
            aux.picture = "\\Logos-Canal\\showsportIcono";
            items.Add(aux);

            aux = new FindCardItem();
            aux.id = 3;
            aux.type = (int)CGClientConstants.FindCardItemType.WINNER_CARD;
            aux.picture = "\\greenscreens\\man1";
            items.Add(aux);

            HandballMatch.getInstance().gameshowFindCardItems = items;

        }

        //public static List<ListViewItem> fillListviewGameshowFindCardItems()
        public static ListViewItem[] fillListviewGameshowFindCardItems()
        {
            ListViewItem[] list = new ListViewItem[HandballMatch.getInstance().gameshowFindCardItems.Count];
            string[] arr;
            int i = 0;
            foreach (FindCardItem item in HandballMatch.getInstance().gameshowFindCardItems)
            {
                arr = new string[] { item.id.ToString(), item.type.ToString(), item.picture };
                list[i] = new ListViewItem(arr);
                i++;
            }
            return list;
        }

        public static Object[] fillComboboxGameshowFindCardMatches()
        {
            Object[] list = new Object[HandballMatch.getInstance().gameshowFindCardMatches.Count];
            string arr;
            int i = 0;
            foreach (FindCardMatch item in HandballMatch.getInstance().gameshowFindCardMatches)
            {
                //    arr = item.uid.ToString() + " [" + item.dateTime.ToShortDateString() + "]";
                arr = item.uid.ToString();
                list[i] = arr;
                i++;
            }
            return list;
        }
    }
}
