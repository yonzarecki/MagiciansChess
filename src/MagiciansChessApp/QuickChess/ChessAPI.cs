using ChessLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Xml;

namespace MagiciansChessAPI.QuickChess
{
    public class ChessAPI
    {
        public const string colorBlack = "black";
        public const string colorWhite = "white";

        private static Game getGameFromXML(string gameXml)
        {
            Game g = new Game();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(gameXml);
            XmlNode newNode = doc.DocumentElement;
            g.XmlDeserialize(newNode); // change Game to contain the XML's content

            return g;
        }

        private static Player getRelaventPlayer(Game g, string playerColor)
        {
            Player p = null;
            switch (playerColor)
            {
                case colorBlack:
                    p = g.BlackPlayer;
                    break;
                case colorWhite:
                    p = g.WhitePlayer;
                    break;
                default:
                    throw new ArgumentException("Only black and white players !");
            }

            if (g.ActivePlay.PlayerSide != p.PlayerSide)  // should be the same player 
                throw new ArgumentException("player color should match the active player");

            return p;
        }

        public static XmlNode getInitialGameXML()
        {
            Game g = new Game();
            g.Reset();
            XmlDocument doc = new XmlDocument();
            return g.XmlSerialize(doc);
        }

        public static Game xmlDeserialize(string gameXML)
        {
            Game g = new Game();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(gameXML);
            XmlNode newNode = doc.DocumentElement;
            g.XmlDeserialize(newNode);
            return g;
        }

        /// <summary>
        ///  calculates the best next move with the current game state for the player
        /// </summary>
        /// <param name="GameXml"> GameXML includes the board info and rules, all wrapped in an XML</param>
        /// <param name="playerColor"> the color of the player we want to calculate its next move</param>
        /// <param name="timeLimitInSecs">time limit for calculation in seconds </param>
        /// <returns> Pair containing the next state Game XML in string and the next move </returns>
        public static Move calculateBestMove(string GameXml, string playerColor, int timeLimitInSecs)
        {
            Game g = getGameFromXML(GameXml);
            Player p = getRelaventPlayer(g, playerColor);
            p.TotalThinkTime = timeLimitInSecs;  // sets the time

            Move nextMove = p.GetBestMove();
            g.DoMove(nextMove); // update board with new move.
            XmlDocument doc = new XmlDocument();

            return nextMove;
        }
    }
}