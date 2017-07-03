using ChessLibrary;
using MagiciansChessAPI.QuickChess;
using MagiciansChessDataAPI.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace MagiciansChessAPI.Controllers
{
    [HttpOperationExceptionFilterAttribute]
    public class ChessAIController : ApiController
    {

        // POST: api/ChessAI
        public string Post(StringAux gameXml, int timeLimitInSecs)
        {
            if (gameXml == null)
                return "WTF";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(gameXml.Str.Replace('"', '\''));
            XmlNode gameXmlNode = doc.DocumentElement;
            ChessLibrary.Game g1 = new ChessLibrary.Game();
            g1.XmlDeserialize(gameXmlNode);

            Player p = g1.BlackPlayer;
            if (g1.BlackTurn())
                p = g1.WhitePlayer;
            ChessLibrary.Move m = p.GetBestMove();
            string best_str = m.ToString2();
            if (g1.DoMove("A6", "A5") == -1)
                return "bad move";
            return best_str;
            /*XmlDocument doc = new XmlDocument();
            doc.LoadXml(gameXml.Str);
            XmlNode gameXmlNode = doc.DocumentElement;

            Move bestMove = ChessAPI.calculateBestMoveXmlNode(gameXmlNode, timeLimitInSecs);*/

            /*System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Move));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            writer.Serialize(sw, bestMove);*/
            //return bestMove.ToString(); // seralized move
        }


    }
    public class StringAux
    {
        public string Str;
        public StringAux(string str)
        {
            this.Str = str;
        }
    }
}
