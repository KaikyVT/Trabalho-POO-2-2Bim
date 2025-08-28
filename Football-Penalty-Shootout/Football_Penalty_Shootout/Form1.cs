
namespace Football_Penalty_Shootout
{
    // Made by MOO ICT
    // For educational purpose only
    public partial class Form1 : Form
    {

        Queue<string> KeeperPosition = new Queue<string>(new[] { "left", "right", "top", "topLeft", "topRight" });

        List<PictureBox> goalTarget;
        int ballX = 0;
        int ballY = 0;
        int goal = 0;
        int miss = 0;
        string state;
        string playerTarget;
        bool aimSet = false;
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            goalTarget = new List<PictureBox> { left, right, top, topLeft, topRight};
        }

        private void AddFila(Queue<string> q, string state)
        {
            if (q.Count >= 10)
            {
                q.Dequeue();
                return;
            }
            else {
                q.Enqueue(state);
            }
                
        }

        // Modificado para adicionar as escolhas do jogador na lista
        // Princípio de uma análise estatística do goleiro
        private void SetGoalTargetEvent(object sender, EventArgs e)
        {
            if (aimSet == true) { return; }

            BallTimer.Start();
            KeeperTimer.Start();
            ChangeGoalKeeperImage();    

            var senderObject = (PictureBox)sender;
            senderObject.BackColor = Color.Beige;

            if (senderObject.Tag == null) return;


            switch (senderObject.Tag.ToString())
            {
                case "topRight":
                    ballX = -7;
                    ballY = 15;
                    playerTarget = senderObject.Tag.ToString();
                    aimSet = true;
                    AddFila(KeeperPosition, "topRight");
                    break;
                case "right":
                    ballX = -11;
                    ballY = 15;
                    playerTarget = senderObject.Tag.ToString();
                    aimSet = true;
                    AddFila(KeeperPosition, "right");
                    break;
                case "top":
                    ballX = 0;
                    ballY = 20;
                    playerTarget = senderObject.Tag.ToString();
                    aimSet = true;
                    AddFila(KeeperPosition, "top");
                    break;
                case "topLeft":
                    ballX = 8;
                    ballY = 15;
                    playerTarget = senderObject.Tag.ToString();
                    aimSet = true;
                    AddFila(KeeperPosition, "topLeft");
                    break;
                default:
                    ballX = 7;
                    ballY = 8;
                    playerTarget = senderObject.Tag.ToString();
                    aimSet = true;
                    AddFila(KeeperPosition, "left");
                    break;
            }

            CheckScore();
            MessageBox.Show(String.Join(", ", KeeperPosition));

        }

        private void KeeperTimerEvent(object sender, EventArgs e)
        {
            switch (state)
            {
                case "left":
                    goalKeeper.Left -= 10;
                    if (goalKeeper.Left < 300) // limite mínimo
                    {
                        ResetKeeper();
                    }
                    break;
                case "right":
                    goalKeeper.Left += 10;
                    if (goalKeeper.Left > 650) // limite máximo
                    {
                        ResetKeeper();
                    }
                    break;
                case "top":
                    goalKeeper.Top -= 10;
                    if (goalKeeper.Top < 120) // limite do alto
                    {
                        ResetKeeper();
                    }
                    break;
                case "topLeft":
                    goalKeeper.Left -= 10;
                    goalKeeper.Top -= 3;
                    if (goalKeeper.Left < 300 || goalKeeper.Top < 120)
                    {
                        ResetKeeper();
                    }
                    break;
                case "topRight":
                    goalKeeper.Left += 10;
                    goalKeeper.Top -= 3;
                    if (goalKeeper.Left > 650 || goalKeeper.Top < 120)
                    {
                        ResetKeeper();
                    }
                    break;
            }
        }

        private void ResetKeeper()
        {
            KeeperTimer.Stop();
            goalKeeper.Location = new Point(478, 225); // posição inicial
            goalKeeper.Image = Properties.Resources.stand_small;
            state = "stand"; // novo estado neutro
        }


        private void BallTimerEvent(object sender, EventArgs e)
        {
            football.Left -= ballX;
            football.Top -= ballY;

            foreach (PictureBox x in goalTarget)
            {
                if (football.Bounds.IntersectsWith(x.Bounds))
                {
                    football.Location = new Point(430, 500);
                    ballX = 0;
                    ballY = 0;
                    aimSet = false;
                    BallTimer.Stop();
                    return;
                }
            }

            // segurança: se a bola sair da tela, resetar também
            if (football.Top < 50 || football.Left < 100 || football.Left > 800)
            {
                football.Location = new Point(430, 500);
                ballX = 0;
                ballY = 0;
                aimSet = false;
                BallTimer.Stop();
            }
        }


        private void CheckScore()
        {
            if (state == playerTarget)
            {
                miss++;
                lblMissed.Text = "Missed: " + miss;
            }
            else
            {
                goal++;
                lblScore.Text = "Scored: " + goal;
            }
        }

        private void ChangeGoalKeeperImage()
        {
            KeeperTimer.Start();
            int i = random.Next(0, KeeperPosition.Count);

            // Como Queue<T> não suporta indexação direta, converta para array ou lista temporária:
            var keeperPositionsArray = KeeperPosition.ToArray();
            state = keeperPositionsArray[i];

            switch (i)
            {
                case 0:
                    goalKeeper.Image = Properties.Resources.left_save_small;
                    break;
                case 1:
                    goalKeeper.Image = Properties.Resources.right_save_small;
                    break;
                case 2:
                    goalKeeper.Image = Properties.Resources.top_save_small;
                    break;
                case 3:
                    goalKeeper.Image = Properties.Resources.top_left_save_small;
                    break;
                case 4:
                    goalKeeper.Image = Properties.Resources.top_right_save_small;
                    break;
            }
        }

    }
}