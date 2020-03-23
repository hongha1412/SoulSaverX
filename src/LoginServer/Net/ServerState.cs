namespace Server.Ghost
{
    public class ServerState
    {
        /* state
         * 07 - 因使用Bug進行遊戲，帳號已凍結
         * 08 - 因不正當賺取金錢，帳號已凍結
         * 09 - 因口出穢言，帳號已凍結
         * 10 - 因洗頻，帳號已凍結
         * 11 - 帳號暫時被凍結
         * 12 - 帳號已凍結
         * 13 - 帳號錯誤，請重新輸入
         * 14 - 您所輸入的密碼錯誤
         * 29 - 因偵測到不當的遊戲進行方式，1小時內將無法進行遊戲
         * -----------------------------------------------
         * 07 - Account is frozen due to gameplay using hack
         * 08 - Account has been frozen due to improper money earning
         * 09 - Account has been frozen due to rumors
         * 10 - The account has been frozen due to frequency washing
         * 11 - Account is temporarily frozen
         * 12 - Account is frozen
         * 13 - Account number is incorrect, please re-enter
         * 14 - The password you entered is incorrect
         * 29 - The game will not be played within 1 hour due to the detection of improper gameplay.
         */
        public enum LoginState
        {
            OK = 0,
            LOGIN_FAILED = 1, // Login Failed
            BUG_LOCK = 7,
            LOCK_5 = 5,
            LOCK_6 = 6,
            BILLING_LOCK = 8,
            FUCK_LOCK = 9,
            SPAM_LOCK = 10,
            TMPE_LOCK = 11,
            USER_LOCK = 12,
            NO_USERNAME = 13,
            PASSWORD_ERR = 14,
            PASSWORD_ERR2 = 15,
            UNKNOWN_LOGIN_ERROR = 16,
            ID_BLOCK_BUGPLAY2 = 17,
            LOGIN_SERVER_DEAD = 30,
            ID_BLOCK_NONE_ACTIVATION = 31,
            LOGIN_FAIL33 = 33,
            PASSWORD_ERROR = 2,
            HACK_LOCK = 29,
           
        }

        /* state
         * 28 = 此為有年齡限制的頻道，請使用其他頻道
         * 04 = 此ID已連線，請稍後再試
         * else = 網路狀態錯誤
         */
        public enum ChannelState
        {
            OK = 0,
            OTHER = 1,
            CONNECTED_WAITE = 4,
            YEARS_OLD = 28
        }
    }
}