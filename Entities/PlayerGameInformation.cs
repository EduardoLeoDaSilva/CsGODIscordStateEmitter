namespace CsGOStateEmitter.Entities
{
    public class PlayerGameInformation
    {
        public int Id { get; set; }
        public string PlayersSteamId { get; set; }
        public int MatchId { get; set; }
        /// <summary>
        /// caminho do arquivo
        /// </summary>
        public string PathImage { get; set; }
        /// <summary>
        /// Nome da imagem
        /// </summary>
        public string NameImage { get; set; }
        /// <summary>
        /// Processos rodando na máquina
        /// </summary>
        public string ProssesNamesBase64 { get; set; }
        public DateTime CreatedDate { get; set; }

        public Player Players { get; set; }
    }
}
