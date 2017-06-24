using ChessLibrary;
using MagiciansChessAPI.QuickChess;
using MagiciansChessDataAPI.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MagiciansChessAPI.Controllers
{
    [HttpOperationExceptionFilterAttribute]
    public class ChessAIController : ApiController
    {

        // need to unpack the serialization at the client
        // GET: api/GetBestMove
        public Move GetBestMove(string gameXml, string playerColor, int timeLimitInSecs)
        {
            Move bestMove = ChessAPI.calculateBestMove(gameXml, playerColor, timeLimitInSecs);
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Move));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            writer.Serialize(sw, bestMove);
            return bestMove; // seralized move
        }
    }
}
