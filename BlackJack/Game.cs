namespace BlackJack
{
    public partial class Game : Form
    {
        public static int uHitNum = 0;
        public static int dHitNum = 0;
        public static int userSum = 0;
        public static int dealerSum = 0;
        public static List<string> deck = new List<string>();
        public Random random = new Random();

        public Game()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.Text = "BlackJack";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            AddAllCards();
            standBtn.Enabled = false;
        }

        private void AddAllCards()
        {
            string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            char[] suits = { 'C', 'D', 'H', 'S' };

            foreach (string value in values)
            {
                foreach (char suit in suits)
                {
                    deck.Add($"{value}{suit}");
                }
            }
        }

        private void Restart()
        {
            deck.Clear();
            ClearCards();
            uHitNum = 0;
            dHitNum = 0;  
            userSum = 0;
            dealerSum = 0;
            AddAllCards();
            uCardsSum.Text = "0";
            dCardsSum.Text = "0";
            hitBtn.Enabled = true;
            standBtn.Enabled = false; 
        }

        private void ClearCards()
        {
            for (int i = 1; i <= uHitNum; i++)
            {
                string controlName = $"uCard{i}";
                var pictureBox = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                if (pictureBox != null)
                {
                    pictureBox.Image = null;
                }
            }

            for (int i = 1; i <= dHitNum; i++)
            {
                string controlName = $"dCard{i}";
                var pictureBox = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                if (pictureBox != null)
                {
                    pictureBox.Image = null;
                }
            }
        }

        private void hitBtn_Click(object sender, EventArgs e)
        {
            if (uHitNum == 0) { standBtn.Enabled = true; }  

            ++uHitNum;
            int rnd = random.Next(deck.Count);
            string card = deck[rnd];
            deck.RemoveAt(rnd);
            ShowSum(card, true);
            ShowCard(card, true);

            if (userSum > 21)
            {
                MessageBox.Show("You Lost.");
                Restart();
            }

            if (uHitNum == 8) { hitBtn.Enabled = false; }
        }

        private void ShowCard(string card, bool isUser)
        {
            string controlName = isUser ? $"uCard{uHitNum}" : $"dCard{dHitNum}";  

            var pictureBox = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;

            if (pictureBox != null)
            {
                pictureBox.Image = Image.FromFile($"../Resources/Cards/{card}.png");
            }
            else
            {
                MessageBox.Show($"Control {controlName} not found!");
            }
        }

        private void ShowSum(string card, bool isUser)
        {
            string cardValue = card.Substring(0, card.Length - 1);
            int cardPoints = 0;

            if (int.TryParse(cardValue, out int numericValue))
            {
                cardPoints = numericValue;
            }
            else if (cardValue == "J" || cardValue == "Q" || cardValue == "K")
            {
                cardPoints = 10;
            }
            else if (cardValue == "A")
            {
                if (isUser)
                {
                    cardPoints = (userSum + 11 > 21) ? 1 : 11;
                }
                else
                {
                    cardPoints = (dealerSum + 11 > 21) ? 1 : 11;
                }
            }

            if (isUser)
            {
                userSum += cardPoints;
                uCardsSum.Text = userSum.ToString();
            }
            else
            {
                dealerSum += cardPoints;
                dCardsSum.Text = dealerSum.ToString();
            }
        }

        private void standBtn_Click(object sender, EventArgs e)
        {
            hitBtn.Enabled = false;

            while (dealerSum < 17 && dHitNum < 8)
            {
                ++dHitNum;
                int rnd = random.Next(deck.Count);
                string card = deck[rnd];
                deck.RemoveAt(rnd);
                ShowCard(card, false);
                ShowSum(card, false);
            }

            if (dealerSum > 21 || dealerSum < userSum)
            {
                MessageBox.Show("You Won!");
            }
            else
            {
                MessageBox.Show("You Lost!");
            }

            Restart();
        }
    }
}
