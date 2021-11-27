using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{

    public partial class Table : Form
    {

        #region GLOBALS
        //map logic
        string[,] map =
        {
            { "w_Rook", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Rook" },
            { "w_Knight", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Knight" },
            { "w_Bishop", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Bishop" },
            { "w_Queen", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Queen" },
            { "w_King", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_King" },
            { "w_Bishop", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Bishop" },
            { "w_Knight", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Knight" },
            { "w_Rook", "w_Pawn", "-", "-", "-", "-", "b_Pawn", "b_Rook" }
        };
        bool[,] stepList = new bool[8, 8];
        Label[,] labels = new Label[8, 8];
        char[] mapColor =
        {
            'b', 'w', 'b', 'w', 'b', 'w', 'b', 'w',
            'w', 'b', 'w', 'b', 'w', 'b', 'w', 'b',
            'b', 'w', 'b', 'w', 'b', 'w', 'b', 'w',
            'w', 'b', 'w', 'b', 'w', 'b', 'w', 'b',
            'b', 'w', 'b', 'w', 'b', 'w', 'b', 'w',
            'w', 'b', 'w', 'b', 'w', 'b', 'w', 'b',
            'b', 'w', 'b', 'w', 'b', 'w', 'b', 'w',
            'w', 'b', 'w', 'b', 'w', 'b', 'w', 'b',
        };
        int X, Y;
        int[] timerAdd = new int[3];
        Label lastLabel;

        //unit logic
        bool selectPhase = true;
        bool isInChess = false;
        string selectedFigure = "";
        string sidePrefix = "w_";
        int[] blackKingPos = { 4, 7 };
        int[] whiteKingPos = { 4, 0 };
        List<string> blackLostUnits = new List<string>(7);
        List<string> whiteLostUnits = new List<string>(7);
        #endregion

        #region FORM INITIALIZE

        public Table() {
            
            InitializeComponent();
            labels[0, 0] = zone_11;
            labels[1, 0] = zone_21;
            labels[2, 0] = zone_31;
            labels[3, 0] = zone_41;
            labels[4, 0] = zone_51;
            labels[5, 0] = zone_61;
            labels[6, 0] = zone_71;
            labels[7, 0] = zone_81;            
            labels[0, 1] = zone_12;
            labels[1, 1] = zone_22;
            labels[2, 1] = zone_32;
            labels[3, 1] = zone_42;
            labels[4, 1] = zone_52;
            labels[5, 1] = zone_62;
            labels[6, 1] = zone_72;
            labels[7, 1] = zone_82;          
            labels[0, 2] = zone_13;
            labels[1, 2] = zone_23;
            labels[2, 2] = zone_33;
            labels[3, 2] = zone_43;
            labels[4, 2] = zone_53;
            labels[5, 2] = zone_63;
            labels[6, 2] = zone_73;
            labels[7, 2] = zone_83;
            labels[0, 3] = zone_14;
            labels[1, 3] = zone_24;
            labels[2, 3] = zone_34;
            labels[3, 3] = zone_44;
            labels[4, 3] = zone_54;
            labels[5, 3] = zone_64;
            labels[6, 3] = zone_74;
            labels[7, 3] = zone_84;
            labels[0, 4] = zone_15;
            labels[1, 4] = zone_25;
            labels[2, 4] = zone_35;
            labels[3, 4] = zone_45;
            labels[4, 4] = zone_55;
            labels[5, 4] = zone_65;
            labels[6, 4] = zone_75;
            labels[7, 4] = zone_85;
            labels[0, 5] = zone_16;
            labels[1, 5] = zone_26;
            labels[2, 5] = zone_36;
            labels[3, 5] = zone_46;
            labels[4, 5] = zone_56;
            labels[5, 5] = zone_66;
            labels[6, 5] = zone_76;
            labels[7, 5] = zone_86;
            labels[0, 6] = zone_17;
            labels[1, 6] = zone_27;
            labels[2, 6] = zone_37;
            labels[3, 6] = zone_47;
            labels[4, 6] = zone_57;
            labels[5, 6] = zone_67;
            labels[6, 6] = zone_77;
            labels[7, 6] = zone_87;
            labels[0, 7] = zone_18;
            labels[1, 7] = zone_28;
            labels[2, 7] = zone_38;
            labels[3, 7] = zone_48;
            labels[4, 7] = zone_58;
            labels[5, 7] = zone_68;
            labels[6, 7] = zone_78;
            labels[7, 7] = zone_88;
        }

        #endregion

        #region FUNCTIONS

        void RemoveLastImage()
        {
            lastLabel.Image = null;
        }
               
        void SetUpNewIamge(string fileName)
        {

            labels[X, Y].Image = System.Drawing.Image.FromFile("img/" + fileName);
        }
        
        void ResetStepList()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    stepList[i, j] = false; 
        }

        void UnitChange()
        {
            //white pawn at the end (pos Y = 7)
            if (sidePrefix == "w_")
            {
                //do nothing if no valuable white units are down
                if (whiteLostUnits.Count <= 0) return;
                
                if (whiteLostUnits.Contains("w_Queen"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/w_queen.png");
                    map[X, Y] = "w_Queen";
                    whiteLostUnits.Remove("w_Queen");
                    return;
                }
                if (whiteLostUnits.Contains("w_Rook"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/w_rook.png");
                    map[X, Y] = "w_Rook";
                    whiteLostUnits.Remove("w_Rook");
                    return;
                }
                if (whiteLostUnits.Contains("w_Bishop"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/w_bishop.png");
                    map[X, Y] = "w_Bishop";
                    whiteLostUnits.Remove("w_Bishop");
                    return;
                }
                if (whiteLostUnits.Contains("w_Knight"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/w_knight.png");
                    map[X, Y] = "w_Knight";
                    whiteLostUnits.Remove("w_Knight");
                    return;
                }
            }
            //black pawn at the end (pos Y = 0)
            else
            {
                //do nothing if no valuable black units are down
                if (blackLostUnits.Count <= 0) return;

                if (blackLostUnits.Contains("b_Queen"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/b_queen.png");
                    map[X, Y] = "b_Queen";
                    blackLostUnits.Remove("b_Queen");
                    return;
                }
                if (blackLostUnits.Contains("b_Rook"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/b_rook.png");
                    map[X, Y] = "b_Rook";
                    blackLostUnits.Remove("b_Rook");
                    return;
                }
                if (blackLostUnits.Contains("b_Bishop"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/w_bishop.png");
                    map[X, Y] = "b_Bishop";
                    blackLostUnits.Remove("b_Bishop");
                    return;
                }
                if (blackLostUnits.Contains("b_Knight"))
                {
                    labels[X, Y].Image = System.Drawing.Image.FromFile("img/w_knight.png");
                    map[X, Y] = "b_Knight";
                    blackLostUnits.Remove("b_Knight");
                    return;
                }
            }
        }
        
        void SetStepList(string selectedFigure, string otherSidePrefix)
        {
            //set helpers
            int a, b;
            //set a logical checker
            bool canMove = false;
            //set possible step list
            switch (selectedFigure)
            {
                case "Pawn":
                {
                    /******WHITE PAWN*****/
                    if (sidePrefix == "w_")
                    {
                        //check double move
                        if (Y == 1)
                        {
                            if (map[X, Y + 1] == "-")
                            {
                                stepList[X, Y + 1] = true;
                                labels[X, Y + 1].BackColor = SystemColors.HotTrack;
                                canMove = true;
                                if (map[X, Y + 2] == "-")
                                {
                                    stepList[X, Y + 2] = true;
                                    labels[X, Y + 2].BackColor = SystemColors.HotTrack;
                                }
                            }
                        }
                        //no double move
                        else
                        {
                            //simple move
                            if (map[X, Y + 1] == "-")
                            {
                                stepList[X, Y + 1] = true;
                                labels[X, Y + 1].BackColor = SystemColors.HotTrack;
                                canMove = true;
                            }
                        }
                        //hit left
                            if (X > 0 && map[X - 1, Y + 1].Contains("b_"))
                            {
                                labels[X - 1, Y + 1].BackColor = SystemColors.HotTrack;
                                stepList[X - 1, Y + 1] = true;
                                canMove = true;
                            }
                        //hit right
                            if (X < 7 && map[X + 1, Y + 1].Contains("b_"))
                            {
                                labels[X + 1, Y + 1].BackColor = SystemColors.HotTrack;
                                stepList[X + 1, Y + 1] = true;
                                canMove = true;
                            }
                        if (canMove == false)
                        {
                            selectPhase = true;
                            map[X, Y] = "w_Pawn";
                            lbl_message.Text = "This pawn cannot move anywhere. Select another figure!";
                        }
                    }
                    /******BLACK PAWN*****/
                    else
                    {
                        //check double move
                        if (Y == 6)
                        {
                            if (map[X, Y - 1] == "-")
                            {
                                stepList[X, Y - 1] = true;
                                labels[X, Y - 1].BackColor = SystemColors.HotTrack;
                                canMove = true;
                                if (map[X, Y - 2] == "-")
                                {
                                    stepList[X, Y - 2] = true;
                                    labels[X, Y - 2].BackColor = SystemColors.HotTrack;
                                }
                            }
                        }
                        //no double move
                        else
                        {
                            //simple move
                            if (map[X, Y - 1] == "-")
                            {
                                stepList[X, Y - 1] = true;
                                labels[X, Y - 1].BackColor = SystemColors.HotTrack;
                                canMove = true;
                            }
                        }
                        //hit right
                            if (X > 0 && map[X - 1, Y - 1].Contains("w_"))
                            {
                                labels[X - 1, Y - 1].BackColor = SystemColors.HotTrack;
                                stepList[X - 1, Y - 1] = true;
                                canMove = true;
                            }
                        //hit left
                            if (X < 7 && map[X + 1, Y - 1].Contains("w_"))
                            {
                                labels[X + 1, Y - 1].BackColor = SystemColors.HotTrack;
                                stepList[X + 1, Y - 1] = true;
                                canMove = true;
                            }
                        if (canMove == false)
                        {
                            selectPhase = true;
                            map[X, Y] = "b_Pawn";
                            lbl_message.Text = "This pawn cannot move anywhere. Select another figure!";
                        }
                    }

                    return;
                }
                case "Rook": 
                {
                    //check RIGHT
                    a = X + 1;
                        
                    while (a <= 7)
                    {                        
                        if (map[a, Y] == "-") {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                        }
                        else if (map[a, Y].Contains(otherSidePrefix)) {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++;
                    }
                    //check LEFT
                    a = X - 1;
                    while (a >= 0)
                    {
                        if (map[a, Y] == "-") {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                        }
                        else if (map[a, Y].Contains(otherSidePrefix)) {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--;
                    }

                    //check TOP
                    a = Y + 1;
                    while (a <= 7)
                    {
                        if (map[X, a] == "-") {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                        }
                        else if (map[X, a].Contains(otherSidePrefix)) {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++;
                    }
                    //check BOTTOM
                    a = Y - 1;
                    while (a >= 0)
                    {
                        if (map[X, a] == "-") {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                        }
                        else if (map[X, a].Contains(otherSidePrefix)) {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--;
                    }
                    if (canMove == false)
                    {
                        selectPhase = true;
                        map[X, Y] = sidePrefix + "Rook";
                        lbl_message.Text = "This rook cannot move anywhere. Select another figure!";
                    }
                    return;
                }
                case "Knight":
                {
                    //check RIGHT
                    a = X + 2;
                    if (a <= 7)
                    {
                        //side top
                        if (Y < 7 && (map[a, Y + 1] == "-" || map[a, Y + 1].Contains(otherSidePrefix)))
                        {
                            labels[a, Y + 1].BackColor = SystemColors.HotTrack;
                            stepList[a, Y + 1] = true;
                            canMove = true;
                        }
                        //side bottom
                        if (Y > 0 && (map[a, Y - 1] == "-" || map[a, Y - 1].Contains(otherSidePrefix)))
                        {
                            labels[a, Y - 1].BackColor = SystemColors.HotTrack;
                            stepList[a, Y - 1] = true;
                            canMove = true;
                        }
                    }            
                    //check LEFT
                    a = X - 2;
                    if (a >= 0)
                    {
                        //side top
                        if (Y < 7 && (map[a, Y + 1] == "-" || map[a, Y + 1].Contains(otherSidePrefix)))
                        {
                            labels[a, Y + 1].BackColor = SystemColors.HotTrack;
                            stepList[a, Y + 1] = true;
                            canMove = true;
                        }
                        //side bottom
                        if (Y > 0 && (map[a, Y - 1] == "-" || map[a, Y - 1].Contains(otherSidePrefix)))
                        {
                            labels[a, Y - 1].BackColor = SystemColors.HotTrack;
                            stepList[a, Y - 1] = true;
                            canMove = true;
                        }
                    }
                    //check TOP
                    a = Y + 2;
                    if (a <= 7)
                    {
                        //side right
                        if (X < 7 && (map[X + 1, a] == "-" || map[X + 1, a].Contains(otherSidePrefix)))
                        {
                            labels[X + 1, a].BackColor = SystemColors.HotTrack;
                            stepList[X + 1, a] = true;
                            canMove = true;
                        }
                        //side left
                        if (X > 0 && (map[X - 1, a] == "-" || map[X - 1, a].Contains(otherSidePrefix)))
                        {
                            labels[X - 1, a].BackColor = SystemColors.HotTrack;
                            stepList[X - 1, a] = true;
                            canMove = true;
                        }
                    }      
                    //check BOTTOM
                    a = Y - 2;
                    if (a >= 0)
                    {
                        //side right
                        if (X < 7 && (map[X + 1, a] == "-" || map[X + 1, a].Contains(otherSidePrefix)))
                        {
                            labels[X + 1, a].BackColor = SystemColors.HotTrack;
                            stepList[X + 1, a] = true;
                            canMove = true;
                        }
                        //side left
                        if (X > 0 && (map[X - 1, a] == "-" || map[X - 1, a].Contains(otherSidePrefix)))
                        {
                            labels[X - 1, a].BackColor = SystemColors.HotTrack;
                            stepList[X - 1, a] = true;
                            canMove = true;
                        }
                    }

                    if (canMove == false)
                    {
                        selectPhase = true;
                        map[X, Y] = sidePrefix + "Knight";
                        lbl_message.Text = "This knight cannot move anywhere. Select another figure!";
                    }
                    return;
                }
                case "Bishop":
                {
                    //check TOP-RIGHT
                    a = X + 1;
                    b = Y + 1;
                    while (a <= 7 && b <= 7)
                    {
                        if (map[a, b] == "-")
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++; b++;
                    }
                    //check BOTTOM-RIGHT
                    a = X + 1;
                    b = Y - 1;
                    while (a <= 7 && b >= 0)
                    {
                        if (map[a, b] == "-")
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++; b--;
                    }
                    //check TOP-LEFT
                    a = X - 1;
                    b = Y + 1;
                    while (a >= 0 && b <= 7)
                    {
                        if (map[a, b] == "-")
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--; b++;
                    }
                    //check BOTTOM-LEFT
                    a = X - 1;
                    b = Y - 1;
                    while (a >= 0 && b >= 0)
                    {
                        if (map[a, b] == "-")
                        {
                            stepList[a, b] = true;
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            stepList[a, b] = true;
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--; b--;
                    }
                    if (canMove == false)
                    {
                        selectPhase = true;
                        map[X, Y] = sidePrefix + "Bishop";
                        lbl_message.Text = "This bishop cannot move anywhere. Select another figure!";
                    }
                    return;
                }
                case "Queen":
                {
                    //check RIGHT
                    a = X + 1;
                    while (a <= 7)
                    {
                        if (map[a, Y] == "-")
                        {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                        }
                        else if (map[a, Y].Contains(otherSidePrefix))
                        {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++;
                     }
                    //check LEFT
                    a = X - 1;
                    while (a >= 0)
                    {
                        if (map[a, Y] == "-")
                        {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                        }
                        else if (map[a, Y].Contains(otherSidePrefix))
                        {
                            labels[a, Y].BackColor = SystemColors.HotTrack;
                            stepList[a, Y] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--;
                    }
                    //check TOP
                    a = Y + 1;
                    while (a <= 7)
                    {
                        if (map[X, a] == "-")
                        {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                        }
                        else if (map[X, a].Contains(otherSidePrefix))
                        {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++;
                    }
                    //check BOTTOM
                    a = Y - 1;
                    while (a >= 0)
                    {
                        if (map[X, a] == "-")
                        {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                        }
                        else if (map[X, a].Contains(otherSidePrefix))
                        {
                            labels[X, a].BackColor = SystemColors.HotTrack;
                            stepList[X, a] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--;
                    }
                    //check TOP-RIGHT
                    a = X + 1;
                    b = Y + 1;
                    while (a <= 7 && b <= 7)
                    {
                        if (map[a, b] == "-")
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++; b++;
                    }
                    //check BOTTOM-RIGHT
                    a = X + 1;
                    b = Y - 1;
                    while (a <= 7 && b >= 0)
                    {
                        if (map[a, b] == "-") {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                    }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a++; b--;
                    }
                    //check TOP-LEFT
                    a = X - 1;
                    b = Y + 1;
                    while (a >= 0 && b <= 7)
                    {
                        if (map[a, b] == "-")
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--; b++;
                    }
                    //check BOTTOM-LEFT
                    a = X - 1;
                    b = Y - 1;
                    while (a >= 0 && b >= 0)
                    {
                        if (map[a, b] == "-")
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                        }
                        else if (map[a, b].Contains(otherSidePrefix))
                        {
                            labels[a, b].BackColor = SystemColors.HotTrack;
                            stepList[a, b] = true;
                            canMove = true;
                            break;
                        }
                        else break;
                        a--; b--;
                    }
                    if (canMove == false)
                    {
                        selectPhase = true;
                        map[X, Y] = sidePrefix + "Queen";
                        lbl_message.Text = "The queen cannot move anywhere. Select another figure!";
                    }
                return;
                }
                case "King":
                {
                    //check TOP
                    if (Y < 7 && (map[X, Y + 1] == "-" || map[X, Y + 1].Contains(otherSidePrefix)))
                    {
                        labels[X, Y + 1].BackColor = SystemColors.HotTrack;
                        stepList[X, Y + 1] = true;
                        canMove = true;
                    }
                    //check TOP-RIGHT
                    if (Y < 7 && X < 7)
                    {
                        if (map[X + 1, Y + 1] == "-" || map[X + 1, Y + 1].Contains(otherSidePrefix))
                        {
                            labels[X + 1, Y + 1].BackColor = SystemColors.HotTrack;
                            stepList[X + 1, Y + 1] = true;
                            canMove = true;
                        }
                    }
                    //check RIGHT
                    if (X < 7)
                    {
                        if (X < 7 && (map[X + 1, Y] == "-" || map[X + 1, Y].Contains(otherSidePrefix)))
                        {
                            labels[X + 1, Y].BackColor = SystemColors.HotTrack;
                            stepList[X + 1, Y] = true;
                            canMove = true;
                        }
                    }
                    //check BOTTOM-RIGHT
                    if (X < 7 && Y > 0)
                    {
                        if (map[X + 1, Y - 1] == "-" || map[X + 1, Y - 1].Contains(otherSidePrefix))
                        {
                            labels[X + 1, Y - 1].BackColor = SystemColors.HotTrack;
                            stepList[X + 1, Y - 1] = true;
                            canMove = true;
                        }
                    }
                    //check BOTTOM
                    if (Y > 0 && (map[X, Y - 1] == "-" || map[X, Y - 1].Contains(otherSidePrefix)))
                    {
                        labels[X, Y - 1].BackColor = SystemColors.HotTrack;
                        stepList[X, Y - 1] = true;
                        canMove = true;
                    }
                    //check BOTTOM-LEFT
                    if (X > 0 && Y > 0)
                    {
                        if (map[X - 1, Y - 1] == "-" || map[X - 1, Y - 1].Contains(otherSidePrefix))
                        {
                            labels[X - 1, Y - 1].BackColor = SystemColors.HotTrack;
                            stepList[X - 1, Y - 1] = true;
                            canMove = true;
                        }
                    }
                    //check LEFT
                    if (X > 0 && (map[X - 1, Y] == "-" || map[X - 1, Y].Contains(otherSidePrefix)))
                    {
                        labels[X - 1, Y].BackColor = SystemColors.HotTrack;
                        stepList[X - 1, Y] = true;
                        canMove = true;
                    }
                    //check TOP-LEFT
                    if (X > 0 && Y < 7)
                    {
                        if (map[X - 1, Y + 1] == "-" || map[X - 1, Y + 1].Contains(otherSidePrefix))
                        {
                            labels[X - 1, Y + 1].BackColor = SystemColors.HotTrack;
                            stepList[X - 1, Y + 1] = true;
                            canMove = true;
                        }
                    }
                    if (canMove == false)
                    {
                        selectPhase = true;
                        map[X, Y] = sidePrefix + "King";
                        lbl_message.Text = "The king cannot move anywhere. Select another figure!";
                    }
                    return;
                }
            }      
        }

        void ClearColorChanges()
        {
            int c = 0;
            foreach (var label in labels)
            {
                if (mapColor[c] == 'b')
                    label.BackColor = SystemColors.ControlDarkDark;
                else
                    label.BackColor = SystemColors.ControlLightLight;
                c++;
            }
        }

        void SignKingsChess()
        {
            isInChess = false;
            /************WHITE KING************/
            //check KNIGHT
            X = whiteKingPos[0];
            Y = whiteKingPos[1];
            if (X + 1 <= 7 && Y + 2 <= 7 && map[X + 1, Y + 2] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X - 1 >= 0 && Y + 2 <= 7 && map[X - 1, Y + 2] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X + 1 <= 7 && Y - 2 >= 0 && map[X + 1, Y - 2] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X - 1 >= 0 && Y - 2 >= 0 && map[X - 1, Y - 2] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X - 2 >= 0 && Y - 1 >= 0 && map[X - 2, Y - 1] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X - 2 >= 0 && Y + 1 <= 7 && map[X - 2, Y + 1] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X + 2 <= 7 && Y - 1 >= 0 && map[X + 2, Y - 1] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            if (X + 2 <= 7 && Y + 1 <= 7 && map[X + 2, Y + 1] == "b_Knight")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            //check RIGHT
            X = whiteKingPos[0] + 1;
            Y = whiteKingPos[1];
            while (X <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Bishop" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                X++;
            }
            //check LEFT
            X = whiteKingPos[0] - 1;
            Y = whiteKingPos[1];
            while (X >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Bishop" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                X--;
            }
            //check TOP
            X = whiteKingPos[0];
            Y = whiteKingPos[1] + 1;
            while (Y <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Bishop" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                Y++;
            }
            //check BOTTOM
            X = whiteKingPos[0];
            Y = whiteKingPos[1] - 1;
            while (Y >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Bishop" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                Y--;
            }
            //check TOP-RIGHT
            X = whiteKingPos[0] + 1;
            Y = whiteKingPos[1] + 1;
            if (X <= 7 && Y <= 7 && map[X, Y] == "b_Pawn")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            while (X <= 7 && Y <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Rook" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                X++; Y++;
            }
            //check BOTTOM-RIGHT
            X = whiteKingPos[0] + 1;
            Y = whiteKingPos[1] - 1;
            while (X <= 7 && Y >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Rook" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                X++; Y--;
            }
            //check TOP-LEFT
            X = whiteKingPos[0] - 1;
            Y = whiteKingPos[1] + 1;
            if (X >= 0 && Y <= 7 && map[X, Y] == "b_Pawn")
            {
                isInChess = true;
                goto endWhiteCheck;
            }
            while (X >= 0 && Y <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Rook" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                X--; Y++;
            }
            //check BOTTOM-LEFT
            X = whiteKingPos[0] - 1;
            Y = whiteKingPos[1] - 1;
            while (X >= 0 && Y >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("w_")) break;
                    else if (map[X, Y] == "b_Pawn" || map[X, Y] == "b_Knight" || map[X, Y] == "b_Rook" || map[X, Y] == "b_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endWhiteCheck;
                    }
                }
                X--; Y--;
            }

            endWhiteCheck:
            if (isInChess)
                labels[whiteKingPos[0], whiteKingPos[1]].BackColor = Color.Red;
            
            isInChess = false;
            /************BLACK KING************/
            //check KNIGHT
            X = blackKingPos[0];
            Y = blackKingPos[1];
            if (X + 1 <= 7 && Y + 2 <= 7 && map[X + 1, Y + 2] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X - 1 >= 0 && Y + 2 <= 7 && map[X - 1, Y + 2] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X + 1 <= 7 && Y - 2 >= 0 && map[X + 1, Y - 2] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X - 1 >= 0 && Y - 2 >= 0 && map[X - 1, Y - 2] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X - 2 >= 0 && Y - 1 >= 0 && map[X - 2, Y - 1] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X - 2 >= 0 && Y + 1 <= 7 && map[X - 2, Y + 1] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X + 2 <= 7 && Y - 1 >= 0 && map[X + 2, Y - 1] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            if (X + 2 <= 7 && Y + 1 <= 7 && map[X + 2, Y + 1] == "w_Knight")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            //check RIGHT
            X = blackKingPos[0] + 1;
            Y = blackKingPos[1];
            while (X <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Bishop" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                X++;
            }
            //check LEFT
            X = blackKingPos[0] - 1;
            Y = blackKingPos[1];
            while (X >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Bishop" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                X--;
            }
            //check TOP
            X = blackKingPos[0];
            Y = blackKingPos[1] + 1;
            while (Y <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Bishop" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                Y++;
            }
            //check BOTTOM
            X = blackKingPos[0];
            Y = blackKingPos[1] - 1;
            while (Y >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Bishop" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                Y--;
            }
            //check TOP-RIGHT
            X = blackKingPos[0] + 1;
            Y = blackKingPos[1] + 1;
            while (X <= 7 && Y <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Rook" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                X++; Y++;
            }
            //check BOTTOM-RIGHT
            X = blackKingPos[0] + 1;
            Y = blackKingPos[1] - 1;
            if (X <= 7 && Y >= 0 && map[X, Y] == "w_Pawn")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            while (X <= 7 && Y >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Rook" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                X++; Y--;
            }
            //check TOP-LEFT
            X = blackKingPos[0] - 1;
            Y = blackKingPos[1] + 1;
            while (X >= 0 && Y <= 7)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Rook" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                X--; Y++;
            }
            //check BOTTOM-LEFT
            X = blackKingPos[0] - 1;
            Y = blackKingPos[1] - 1;
            if (X >= 0 && Y >= 0 && map[X, Y] == "w_Pawn")
            {
                isInChess = true;
                goto endBlackCheck;
            }
            while (X >= 0 && Y >= 0)
            {
                if (map[X, Y] != "-")
                {
                    if (map[X, Y].Contains("b_")) break;
                    else if (map[X, Y] == "w_Pawn" || map[X, Y] == "w_Knight" || map[X, Y] == "w_Rook" || map[X, Y] == "w_King")
                        break;
                    else
                    {
                        isInChess = true;
                        goto endBlackCheck;
                    }
                }
                X--; Y--;
            }

            endBlackCheck:
            if (isInChess)
                labels[blackKingPos[0], blackKingPos[1]].BackColor = Color.Red;
        }


        #endregion

        #region EVENTS

        private void SelectAndStep(object sender, EventArgs e)
        {
            //get zone number from label text
            int zoneNumber = int.Parse(sender.ToString().Substring(sender.ToString().Length - 2));
            bool defeat = false;
            //set coordinates
            X = (zoneNumber / 10) - 1;
            Y = (zoneNumber % 10) - 1;

            //select other
            string otherSidePrefix;
            if (sidePrefix == "w_") otherSidePrefix = "b_";
            else otherSidePrefix = "w_";

            //***PHASE: SELECT***
            if (selectPhase)
            {
                //empty select, enemy select
                if (map[X, Y] == "-" || map[X, Y].Contains(otherSidePrefix)) return;

                //pawn at the end
                if ((Y == 0 && map[X, Y] == "b_Pawn") || (Y == 7 && map[X, Y] == "w_Pawn")) {

                    if (sidePrefix == "w_" && whiteLostUnits.Count > 0)
                    {
                        UnitChange();
                        return;
                    }
                    else if (sidePrefix == "b_" && blackLostUnits.Count > 0)
                    {
                        UnitChange();
                        return;
                    }
                    lbl_message.Text = "This pawn cannot move further. Select another figure!";
                    return;
                }

                selectedFigure = map[X, Y].Substring(2);
                lastLabel = labels[X, Y];
                map[X, Y] = "-";
                selectPhase = false;
                lbl_message.Text = selectedFigure + " is selected! Make your move!";
                SetStepList(selectedFigure, otherSidePrefix);
            }

            //***PHASE: STEP***
            else
            {
                //deselect
                if (lastLabel == labels[X, Y])
                {
                    lbl_message.Text = "Deselected! Select a new figure!";
                    map[X, Y] = sidePrefix + selectedFigure;
                    selectPhase = true;
                    ClearColorChanges();
                    ResetStepList();
                    SignKingsChess();
                    return;
                }
                //possible steps
                if (!stepList[X, Y])
                {
                    lbl_message.Text = "Cannot move there!";
                    return;
                }
                
                //the step is made, prepare select phase
                //check defeat conditions and lost unit logic
                if (sidePrefix == "w_")
                {
                    switch (map[X, Y])
                    {
                        case "b_King":
                        {
                            for (int i = 0; i < 8; i++)
                                for (int j = 0; j < 8; j++)
                                    map[i, j] = "-";
                            MessageBox.Show("The black king has fallen! White wins!", "Victory");
                            lbl_message.Text = "The black king has fallen! White wins!";
                            defeat = true;
                            break;
                        }
                        case "b_Queen":
                        {
                            blackLostUnits.Add("b_Queen"); break;
                        }
                        case "b_Rook":
                        {
                            blackLostUnits.Add("b_Rook"); break;
                        }
                        case "b_Bishop":
                        {
                            blackLostUnits.Add("b_Bishop"); break;
                        }
                        case "b_Knight":
                        {
                            blackLostUnits.Add("b_Knight"); break;
                        }
                    }
                }
                else if (sidePrefix == "b_")
                {
                    switch (map[X, Y])
                    {
                        case "w_King":
                        {
                            for (int i = 0; i < 8; i++)
                                for (int j = 0; j < 8; j++)
                                    map[i, j] = "-";
                            MessageBox.Show("The white king has fallen! Black wins!", "Victory");
                            lbl_message.Text = "The white king has fallen! Black wins!";
                            defeat = true;
                            break;
                        }
                        case "w_Queen":
                        {
                            whiteLostUnits.Add("w_Queen"); break;
                        }
                        case "w_Rook":
                        {
                            whiteLostUnits.Add("w_Rook"); break;
                        }
                        case "w_Bishop":
                        {
                            whiteLostUnits.Add("w_Bishop"); break;
                        }
                        case "w_Knight":
                        {
                            whiteLostUnits.Add("w_Knight"); break;
                        }
                    }
                }     
                map[X, Y] = sidePrefix + selectedFigure;
                //if the step was made with kings
                if (map[X, Y] == "w_King")
                {
                    whiteKingPos[0] = X;
                    whiteKingPos[1] = Y;
                }
                if (map[X, Y] == "b_King")
                {
                    blackKingPos[0] = X;
                    blackKingPos[1] = Y;
                }
                string fileName = (sidePrefix + selectedFigure.ToLower()) + ".png";
                SetUpNewIamge(fileName);
                RemoveLastImage();
                ResetStepList();
                ClearColorChanges();
                //reset timer
                TimerTable.Text = "00:00:00";
                for (int i = 0; i < 3; i++)
                    timerAdd[i] = 0;
                if (defeat) return;
                selectPhase = true;
                //show turn and pawn change
                if (sidePrefix == "w_")
                {
                    if (Y == 7 && map[X, Y] == "w_Pawn")
                        UnitChange();
                    lbl_message.Text = "Black's turn!";
                    SideColor.BackColor = SystemColors.ActiveCaptionText;
                    sidePrefix = "b_";
                }
                else
                {
                    if (Y == 0 && map[X, Y] == "b_Pawn")
                        UnitChange();
                    lbl_message.Text = "White's turn!";
                    SideColor.BackColor = SystemColors.ControlLightLight;
                    sidePrefix = "w_";
                }
                SignKingsChess();
            }
        }
        
        private void StartNewGame(object sender, EventArgs e)
        {
            //set up colors and globals
            selectPhase = true;
            sidePrefix = "w_";
            lbl_message.Text = "White's turn!";
            SideColor.BackColor = SystemColors.ControlLightLight;
            whiteLostUnits.Clear();
            blackLostUnits.Clear();

            //reset timer
            TimerTable.Text = "00:00:00";
            for (int i = 0; i < 3; i++)
                timerAdd[i] = 0;

            //STEP 1: clear the map
            ClearColorChanges(); //color
            ResetStepList(); //logic
            //clear positions
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    map[i, j] = "-";
            //clear images
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    labels[i, j].Image = null;

            //STEP 2: set up images and positions
            //white
            map[0, 0] = "w_Rook";
            labels[0, 0].Image = System.Drawing.Image.FromFile("img/w_rook.png");
            map[0, 1] = "w_Pawn";
            labels[0, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[1, 0] = "w_Knight";
            labels[1, 0].Image = System.Drawing.Image.FromFile("img/w_knight.png");
            map[1, 1] = "w_Pawn";
            labels[1, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[2, 0] = "w_Bishop";
            labels[2, 0].Image = System.Drawing.Image.FromFile("img/w_bishop.png");
            map[2, 1] = "w_Pawn";
            labels[2, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[3, 0] = "w_Queen";
            labels[3, 0].Image = System.Drawing.Image.FromFile("img/w_queen.png");
            map[3, 1] = "w_Pawn";
            labels[3, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[4, 0] = "w_King";
            labels[4, 0].Image = System.Drawing.Image.FromFile("img/w_king.png");
            map[4, 1] = "w_Pawn";
            labels[4, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[5, 0] = "w_Bishop";
            labels[5, 0].Image = System.Drawing.Image.FromFile("img/w_bishop.png");
            map[5, 1] = "w_Pawn";
            labels[5, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[6, 0] = "w_Knight";
            labels[6, 0].Image = System.Drawing.Image.FromFile("img/w_knight.png");
            map[6, 1] = "w_Pawn";
            labels[6, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            map[7, 0] = "w_Rook";
            labels[7, 0].Image = System.Drawing.Image.FromFile("img/w_rook.png");
            map[7, 1] = "w_Pawn";
            labels[7, 1].Image = System.Drawing.Image.FromFile("img/w_pawn.png");
            //black-------------------
            map[0, 7] = "b_Rook";
            labels[0, 7].Image = System.Drawing.Image.FromFile("img/b_rook.png");
            map[0, 6] = "b_Pawn";
            labels[0, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[1, 7] = "b_Knight";
            labels[1, 7].Image = System.Drawing.Image.FromFile("img/b_knight.png");
            map[1, 6] = "b_Pawn";
            labels[1, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[2, 7] = "b_Bishop";
            labels[2, 7].Image = System.Drawing.Image.FromFile("img/b_bishop.png");
            map[2, 6] = "b_Pawn";
            labels[2, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[3, 7] = "b_Queen";
            labels[3, 7].Image = System.Drawing.Image.FromFile("img/b_queen.png");
            map[3, 6] = "b_Pawn";
            labels[3, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[4, 7] = "b_King";
            labels[4, 7].Image = System.Drawing.Image.FromFile("img/b_king.png");
            map[4, 6] = "b_Pawn";
            labels[4, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[5, 7] = "b_Bishop";
            labels[5, 7].Image = System.Drawing.Image.FromFile("img/b_bishop.png");
            map[5, 6] = "b_Pawn";
            labels[5, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[6, 7] = "b_Knight";
            labels[6, 7].Image = System.Drawing.Image.FromFile("img/b_knight.png");
            map[6, 6] = "b_Pawn";
            labels[6, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
            map[7, 7] = "b_Rook";
            labels[7, 7].Image = System.Drawing.Image.FromFile("img/b_rook.png");
            map[7, 6] = "b_Pawn";
            labels[7, 6].Image = System.Drawing.Image.FromFile("img/b_pawn.png");
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (timerAdd[2] == 60)
            {
                timerAdd[2] = 0;
                timerAdd[1]++;
            }
            if (timerAdd[1] == 60)
            {
                timerAdd[1] = 0;
                timerAdd[0]++;
            }
            if (timerAdd[1] >= 10)
            {
                if (timerAdd[2] >= 10)
                    TimerTable.Text = $"0{timerAdd[0]}:{timerAdd[1]}:{timerAdd[2]++}";
                else
                    TimerTable.Text = $"0{timerAdd[0]}:{timerAdd[1]}:0{timerAdd[2]++}";
            }
            else
            {
                if (timerAdd[2] >= 10)
                    TimerTable.Text = $"0{timerAdd[0]}:0{timerAdd[1]}:{timerAdd[2]++}";
                else
                    TimerTable.Text = $"0{timerAdd[0]}:0{timerAdd[1]}:0{timerAdd[2]++}";
            }
                
        }

        #endregion
    }
}

