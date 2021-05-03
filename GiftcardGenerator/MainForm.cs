using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GiftcardGenerator
{
    public partial class MainForm : Form
    {
        
        Random rand = new Random();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbCardType.SelectedIndex = 0;
            //money.Controls[0].Visible = false;
        }

        private void GetType(string type)
        {
            //Apple
            // Bass Pro
            // Google Play
            // Target
            // Walmart

            switch (type.ToLower())
            {
                case "apple":
                    Generate(true);
                    break;
                case "bass pro":
                    GenerateBassPro();
                    break;
                case "google play":
                    GenerateGoogle();
                    break;
                case "target":
                    GenerateTarget();
                    break;
                case "walmart":
                    GenerateWalmart();
                    break;
                default:
                    Generate(false);
                    break;
            }
        }

        private void GenerateBassPro()
        {
            for (int i = 0; i < 5; i++)
            {
                string card = "";
                for (int j = 0; j < 19; j++)
                {
                    card += rand.Next(0, 10);
                    if (j % 3 == 0) card += " ";
                }

                card = card.Remove(card.Length - 1, 1);

                string pin = rand.Next(1000, 9999).ToString();

                lbGiftcardNumbers.AppendText(card + "\tPin: " + pin + Environment.NewLine);
                
            }
        }

        private void GenerateWalmart()
        {
            for (int i = 0; i < 5; i++)
            {
                string card = "";
                for (int j = 0; j < 5; j++)
                {
                    card += rand.Next(1000, 10000);
                    card += "-";
                }

                card = card.Remove(card.Length - 1, 1);

                string pin = rand.Next(1000, 9999).ToString();

                lbGiftcardNumbers.AppendText(card + "\tPin: " + pin + Environment.NewLine);
                
            }
        }

        /// <summary>
        ///  Generate target gift cards using the rules to create the proper value that fits the chrome redeem extension.
        /// </summary>
        private void GenerateTarget()
        {
            for (int cards = 0; cards < 5; cards++)
            {
                //generate the giftcard number first
                string card = "";
                for (int i = 0; i < 5; i++)
                {
                    card += rand.Next(10);
                    card += rand.Next(10);
                    card += rand.Next(10);
                    card += "-";

                }
                card = card.Remove(card.Length - 1, 1); //remove trailing -

                //Now generate the pin
                string pin = "";
                for (int i = 0; i < 4; i++) { pin += rand.Next(10); } //first four numbers set.

                //custom ammount less than 1,000 and not an even amount.
                if (money.Value < 1000 && !money.Value.ToString().EndsWith("00"))
                {
                    pin += rand.Next(2, 10);
                    pin += money.Value.ToString().PadLeft(3, '0');
                }
                else if (money.Value >= 1000)
                {
                    //thousands
                    pin += "1";
                    pin += money.Value.ToString()[0];
                    pin += rand.Next(10);
                    pin += rand.Next(10);
                }
                else
                {
                    //hundreds
                    pin += "0";
                    pin += money.Value.ToString()[0];
                    pin += rand.Next(10);
                    pin += rand.Next(10);

                }

                lbGiftcardNumbers.AppendText(card + "\tPin: " + pin + Environment.NewLine);

            }

        }

        /// <summary>
        /// Generate Google Play Cards that follow the rules of the chrome redeem extension
        /// </summary>
        private void GenerateGoogle()
        {
            for (int g = 0; g < 5; g++)
            {
                string value = money.Value.ToString().PadLeft(4, '0');
                var builder = new StringBuilder("AAAAAAAAAAAAAAAA"); //cards are 16 characters.
                for (int i = 0; i < builder.Length; ++i)
                {
                    builder[i] = GetLetter();
                }

                //replace random characters with our money value.
                if (money.Value >= 1000) { builder[rand.Next(0, 4)] = value[0]; }
                if (money.Value >= 100) { builder[rand.Next(4, 8)] = value[1]; }
                if (money.Value >= 10) { builder[rand.Next(8, 12)] = value[2]; }
                if (money.Value > 0) { builder[rand.Next(12, 16)] = value[3]; }

                lbGiftcardNumbers.AppendText(builder.ToString() + Environment.NewLine);
            }

            char GetLetter()
            {
                char[] chars = "ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();
                int i = rand.Next(chars.Length);
                return chars[i];
            }
        }

        private void Generate(bool withPin)
        {
            if (!withPin)
            {
                for (int i = 0; i < 5; i++)
                {
                    lbGiftcardNumbers.AppendText(GenerateCard() + Environment.NewLine);
                    //lbGiftcardNumbers.Items.Add(GenerateCard());
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    lbGiftcardNumbers.AppendText(GenerateCard() + "\t\tPin: " + GeneratePin() + Environment.NewLine);
                }
            }
        }

        private string GeneratePin()
        {
            string pin = "";
            for (int i = 0; i < 4; i++)
            {
                pin += rand.Next(0, 10);
            }

            return pin;
        }

        private string GenerateCard()
        {
            string cardNumber = "";

            for (int i = 0; i < 16; i++)
            {
                if (i % 4 == 0) cardNumber += " ";
                int numOrChar = rand.Next(0, 2);
                if (numOrChar >= 1)
                {
                    cardNumber += rand.Next(0, 10);
                }
                else
                {
                    cardNumber += Convert.ToChar(rand.Next(65, 90));
                }
            }

            return cardNumber;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            lbGiftcardNumbers.Text = "";
            GetType(cbCardType.Text);
        }
    }
}
