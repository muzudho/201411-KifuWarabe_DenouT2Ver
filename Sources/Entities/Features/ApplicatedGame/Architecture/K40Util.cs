namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    public abstract class K40Util
    {

        /// <summary>
        /// 駒ハンドルが有効か否かを判定します。
        /// </summary>
        /// <param name="komaHandle"></param>
        /// <returns></returns>
        public static bool OnKoma(int komaHandle)
        {
            return 0 <= komaHandle && komaHandle <= 39;
        }

    }
}
