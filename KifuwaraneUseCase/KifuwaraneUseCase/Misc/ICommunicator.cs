namespace KifuwaraneUseCase.Misc
{
    public interface ICommunicator
    {
        /// <summary>
        /// 将棋所に向かってメッセージを送り返すとともに、
        /// ログ・ファイルに通信を記録します。
        /// </summary>
        /// <param name="line"></param>
        void Send(string line);
    }
}
