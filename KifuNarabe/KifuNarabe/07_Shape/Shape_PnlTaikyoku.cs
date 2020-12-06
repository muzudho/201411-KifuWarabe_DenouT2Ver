using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneGui.L07_Shape
{


    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。１つの対局で描かれるものは、ここにまとめて入れられています。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_PnlTaikyoku : Shape_Abstract
    {


        #region プロパティ類

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 出力切替。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SyuturyokuKirikae SyuturyokuKirikae
        {
            get
            {
                return this.syuturyokuKirikae;
            }
        }

        public void SetSyuturyokuKirikae(SyuturyokuKirikae value)
        {
            this.syuturyokuKirikae = value;
        }

        private SyuturyokuKirikae syuturyokuKirikae;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// つまんでいる駒
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int HTumandeiruKoma
        {
            get
            {
                return this.hTumandeiruKoma;
            }
        }

        public void SetHTumandeiruKoma(int value)
        {
            this.hTumandeiruKoma = value;
        }
        private int hTumandeiruKoma;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かし終わった駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public K40 MovedKoma
        {
            get
            {
                return this.movedKoma;
            }
        }

        public void SetHMovedKoma(K40 value)
        {
            this.movedKoma = value;
        }

        private K40 movedKoma;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool SelectFirstTouch
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成るフラグ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         マウスボタン押下時にセットされ、
        ///         マウスボタンを放したときに読み取られます。
        /// 
        /// </summary>
        public bool Naru
        {
            get
            {
                return this.naruFlag;
            }
        }

        public void SetNaruFlag(bool naru)
        {
            this.naruFlag = naru;
        }
        private bool naruFlag;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 要求：　成る／成らないダイアログボックス
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///     0: なし
        ///     1: 成るか成らないかボタンを表示して決定待ち中。
        /// 
        /// </summary>
        public bool Requested_NaruDialogToShow
        {
            get
            {
                return this.requested_NaruDialogToShow;
            }
        }

        public void Request_NaruDialogToShow(bool show)
        {
            this.requested_NaruDialogToShow = show;
        }
        private bool requested_NaruDialogToShow;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成る、で移動先
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnMasu NaruBtnMasu
        {
            get
            {
                return this.naruBtnMasu;
            }
        }

        public void SetNaruMasu(Shape_BtnMasu naruMasu)
        {
            this.naruBtnMasu = naruMasu;
        }
        private Shape_BtnMasu naruBtnMasu;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// マウスで駒を動かしたときに使います。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 棋譜再生中は使いません。
        /// 
        /// </summary>
        public IKomaPos MousePosOrNull2 { get { return this.mousePosOrNull2; } }
        public void SetMousePosOrNull2(IKomaPos mousePos) { this.mousePosOrNull2 = mousePos; }
        private IKomaPos mousePosOrNull2;

        public Ks14 MousePos_TottaKomaSyurui { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒ボタンの配列。局面のKomaDoorsと同じ添え字で一対一対応。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///     *Doors…名前の由来：ハンドル１つに対応するから。
        /// 
        /// </summary>
        public Shape_BtnKoma[] BtnKomaDoors
        {
            get
            {
                return this.btnKomaDoors;
            }
        }
        //public Shape_BtnKoma BtnKomaDoors(K40 koma)
        //{
        //    return this.btnKomaDoors[(int)koma];
        //}
        public void SetBtnKomaDoors(Shape_BtnKoma[] btnKomaDoors)
        {
            this.btnKomaDoors = btnKomaDoors;
        }
        private Shape_BtnKoma[] btnKomaDoors;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋盤
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlShogiban Shogiban
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒置き。[0]先手、[1]後手、[2]駒袋。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlKomadai[] KomadaiArr
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 差し手符号。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="fugo"></param>
        /// <param name="memberName"></param>
        public void SetFugo(
            string fugo,
            [CallerMemberName] string memberName = ""
            )
        {
            this.lblFugo.Text = fugo;

            //System.Console.WriteLine("符号："+fugo+"："+memberName);
        }
        private Shape_LblBox lblFugo;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後表示。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="sengo"></param>
        private Shape_LblBox lblSengo;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成るボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnNaru
        {
            get
            {
                return this.btnNaru;
            }
        }
        private Shape_BtnBox btnNaru;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成らないボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnNaranai
        {
            get
            {
                return this.btnNaranai;
            }
        }
        private Shape_BtnBox btnNaranai;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 向きボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnMuki
        {
            get
            {
                return this.btnMuki;
            }
        }
        private Shape_BtnBox btnMuki;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 初期配置ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnSyokihaichi
        {
            get
            {
                return this.btnSyokihaichi;
            }
        }
        private Shape_BtnBox btnSyokihaichi;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// クリアーボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnClear
        {
            get
            {
                return this.btnClear;
            }
        }
        private Shape_BtnBox btnClear;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 再生ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnPlay
        {
            get
            {
                return this.btnPlay;
            }
        }
        private Shape_BtnBox btnPlay;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// コマ送りボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnForward
        {
            get
            {
                return this.btnForward;
            }
        }
        private Shape_BtnBox btnForward;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 戻る
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnBackward
        {
            get
            {
                return this.btnBackward;
            }
        }
        private Shape_BtnBox btnBackward;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋エンジン起動
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnShogiEngineKido
        {
            get
            {
                return this.btnShogiEngineKido;
            }
        }
        private Shape_BtnBox btnShogiEngineKido;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 出力切替ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnSyuturyokuKirikae
        {
            get
            {
                return this.btnSyuturyokuKirikae;
            }
        }
        private Shape_BtnBox btnSyuturyokuKirikae;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [▲]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Sente
        {
            get
            {
                return this.btnFugo_Sente;
            }
        }
        private Shape_BtnBox btnFugo_Sente;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [△]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Gote
        {
            get
            {
                return this.btnFugo_Gote;
            }
        }
        private Shape_BtnBox btnFugo_Gote;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [１]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_1
        {
            get
            {
                return this.btnFugo_1;
            }
        }
        private Shape_BtnBox btnFugo_1;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [２]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_2
        {
            get
            {
                return this.btnFugo_2;
            }
        }
        private Shape_BtnBox btnFugo_2;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [３]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_3
        {
            get
            {
                return this.btnFugo_3;
            }
        }
        private Shape_BtnBox btnFugo_3;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [４]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_4
        {
            get
            {
                return this.btnFugo_4;
            }
        }
        private Shape_BtnBox btnFugo_4;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [５]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_5
        {
            get
            {
                return this.btnFugo_5;
            }
        }
        private Shape_BtnBox btnFugo_5;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [６]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_6
        {
            get
            {
                return this.btnFugo_6;
            }
        }
        private Shape_BtnBox btnFugo_6;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [７]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_7
        {
            get
            {
                return this.btnFugo_7;
            }
        }
        private Shape_BtnBox btnFugo_7;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [８]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_8
        {
            get
            {
                return this.btnFugo_8;
            }
        }
        private Shape_BtnBox btnFugo_8;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [９]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_9
        {
            get
            {
                return this.btnFugo_9;
            }
        }
        private Shape_BtnBox btnFugo_9;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [同]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Dou
        {
            get
            {
                return this.btnFugo_Dou;
            }
        }
        private Shape_BtnBox btnFugo_Dou;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [歩]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Fu
        {
            get
            {
                return this.btnFugo_Fu;
            }
        }
        private Shape_BtnBox btnFugo_Fu;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [飛]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Hisya
        {
            get
            {
                return this.btnFugo_Hisya;
            }
        }
        private Shape_BtnBox btnFugo_Hisya;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [角]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Kaku
        {
            get
            {
                return this.btnFugo_Kaku;
            }
        }
        private Shape_BtnBox btnFugo_Kaku;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [香]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Kyo
        {
            get
            {
                return this.btnFugo_Kyo;
            }
        }
        private Shape_BtnBox btnFugo_Kyo;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [桂]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Kei
        {
            get
            {
                return this.btnFugo_Kei;
            }
        }
        private Shape_BtnBox btnFugo_Kei;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [銀]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Gin
        {
            get
            {
                return this.btnFugo_Gin;
            }
        }
        private Shape_BtnBox btnFugo_Gin;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [金]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Kin
        {
            get
            {
                return this.btnFugo_Kin;
            }
        }
        private Shape_BtnBox btnFugo_Kin;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [王]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Oh
        {
            get
            {
                return this.btnFugo_Oh;
            }
        }
        private Shape_BtnBox btnFugo_Oh;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [玉]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Gyoku
        {
            get
            {
                return this.btnFugo_Gyoku;
            }
        }
        private Shape_BtnBox btnFugo_Gyoku;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [と]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Tokin
        {
            get
            {
                return this.btnFugo_Tokin;
            }
        }
        private Shape_BtnBox btnFugo_Tokin;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [成香]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Narikyo
        {
            get
            {
                return this.btnFugo_Narikyo;
            }
        }
        private Shape_BtnBox btnFugo_Narikyo;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [成桂]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Narikei
        {
            get
            {
                return this.btnFugo_Narikei;
            }
        }
        private Shape_BtnBox btnFugo_Narikei;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [成銀]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Narigin
        {
            get
            {
                return this.btnFugo_Narigin;
            }
        }
        private Shape_BtnBox btnFugo_Narigin;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [竜]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Ryu
        {
            get
            {
                return this.btnFugo_Ryu;
            }
        }
        private Shape_BtnBox btnFugo_Ryu;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [馬]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Uma
        {
            get
            {
                return this.btnFugo_Uma;
            }
        }
        private Shape_BtnBox btnFugo_Uma;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [寄]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Yoru
        {
            get
            {
                return this.btnFugo_Yoru;
            }
        }
        private Shape_BtnBox btnFugo_Yoru;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [引]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Hiku
        {
            get
            {
                return this.btnFugo_Hiku;
            }
        }
        private Shape_BtnBox btnFugo_Hiku;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [上]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Agaru
        {
            get
            {
                return this.btnFugo_Agaru;
            }
        }
        private Shape_BtnBox btnFugo_Agaru;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [右]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Migi
        {
            get
            {
                return this.btnFugo_Migi;
            }
        }
        private Shape_BtnBox btnFugo_Migi;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [左]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Hidari
        {
            get
            {
                return this.btnFugo_Hidari;
            }
        }
        private Shape_BtnBox btnFugo_Hidari;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [直]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Sugu
        {
            get
            {
                return this.btnFugo_Sugu;
            }
        }
        private Shape_BtnBox btnFugo_Sugu;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [成]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Nari
        {
            get
            {
                return this.btnFugo_Nari;
            }
        }
        private Shape_BtnBox btnFugo_Nari;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [不成]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Funari
        {
            get
            {
                return this.btnFugo_Funari;
            }
        }
        private Shape_BtnBox btnFugo_Funari;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [打]符号ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Da
        {
            get
            {
                return this.btnFugo_Da;
            }
        }
        private Shape_BtnBox btnFugo_Da;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [全消]ボタン     ＜入力欄用＞
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_Zenkesi
        {
            get
            {
                return this.btnFugo_Zenkesi;
            }
        }
        private Shape_BtnBox btnFugo_Zenkesi;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [ここから採譜]ボタン
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnBox BtnFugo_KokokaraSaifu
        {
            get
            {
                return this.btnFugo_KokokaraSaifu;
            }
        }
        private Shape_BtnBox btnFugo_KokokaraSaifu;
        //------------------------------------------------------------
        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Shape_PnlTaikyoku()
        {
            // 初期化
            System.Console.WriteLine("つまんでいる駒を放します。(1)");
            this.SetHTumandeiruKoma(-1);
            this.SetHMovedKoma(K40.Error);
            
            //----------
            // 出力切替
            //----------
            this.syuturyokuKirikae = SyuturyokuKirikae.Sfen;


            //----------
            // 成る成らないダイアログ
            //----------
            this.Request_NaruDialogToShow(false);

            //----------
            // 将ボタン
            //----------
            this.SetBtnKomaDoors( new Shape_BtnKoma[]{

                new Shape_BtnKoma(K40.SenteOh),//[0]
                new Shape_BtnKoma(K40.GoteOh),

                new Shape_BtnKoma(K40.Hi1),
                new Shape_BtnKoma(K40.Hi2),

                new Shape_BtnKoma(K40.Kaku1),
                new Shape_BtnKoma(K40.Kaku2),//[5]

                new Shape_BtnKoma(K40.Kin1),
                new Shape_BtnKoma(K40.Kin2),
                new Shape_BtnKoma(K40.Kin3),
                new Shape_BtnKoma(K40.Kin4),

                new Shape_BtnKoma(K40.Gin1),//[10]
                new Shape_BtnKoma(K40.Gin2),
                new Shape_BtnKoma(K40.Gin3),
                new Shape_BtnKoma(K40.Gin4),

                new Shape_BtnKoma(K40.Kei1),
                new Shape_BtnKoma(K40.Kei2),//[15]
                new Shape_BtnKoma(K40.Kei3),
                new Shape_BtnKoma(K40.Kei4),

                new Shape_BtnKoma(K40.Kyo1),
                new Shape_BtnKoma(K40.Kyo2),
                new Shape_BtnKoma(K40.Kyo3),//[20]
                new Shape_BtnKoma(K40.Kyo4),

                new Shape_BtnKoma(K40.Fu1),
                new Shape_BtnKoma(K40.Fu2),
                new Shape_BtnKoma(K40.Fu3),
                new Shape_BtnKoma(K40.Fu4),//[25]
                new Shape_BtnKoma(K40.Fu5),
                new Shape_BtnKoma(K40.Fu6),
                new Shape_BtnKoma(K40.Fu7),
                new Shape_BtnKoma(K40.Fu8),
                new Shape_BtnKoma(K40.Fu9),//[30]

                new Shape_BtnKoma(K40.Fu10),
                new Shape_BtnKoma(K40.Fu11),
                new Shape_BtnKoma(K40.Fu12),
                new Shape_BtnKoma(K40.Fu13),
                new Shape_BtnKoma(K40.Fu14),//[35]
                new Shape_BtnKoma(K40.Fu15),
                new Shape_BtnKoma(K40.Fu16),
                new Shape_BtnKoma(K40.Fu17),
                new Shape_BtnKoma(K40.Fu18)//[39]
            });

            //----------
            // 将棋盤
            //----------
            this.Shogiban = new Shape_PnlShogiban(200, 220);

            //----------
            // 駒置き
            //----------
            this.KomadaiArr = new Shape_PnlKomadai[3];
            this.KomadaiArr[0] = new Shape_PnlKomadai( Okiba.Sente_Komadai, 610, 220, Sengo.Sente);
            this.KomadaiArr[1] = new Shape_PnlKomadai( Okiba.Gote_Komadai, 10, 170, Sengo.Gote);
            this.KomadaiArr[2] = new Shape_PnlKomadai(Okiba.KomaBukuro, 810, 220, Sengo.Sente);

            //----------
            // 符号表示
            //----------
            this.lblFugo = new Shape_LblBox("符号", 480, 145);

            //----------
            // 先後表示
            //----------
            this.lblSengo = new Shape_LblBox("－－", 350, 145);

            //----------
            // 成るボタン
            //----------
            //this.btnNaru = new Shape_BtnBox("成る", 140, 290);//140, 590
            this.btnNaru = new Shape_BtnBox("成る", 140, 590);
            this.btnNaru.BackColor = Color.Yellow;
            this.btnNaru.Visible = false;

            //----------
            // 成らないボタン
            //----------
            //this.btnNaranai = new Shape_BtnBox("成らない", 310, 290);//210, 590
            this.btnNaranai = new Shape_BtnBox("成らない", 210, 590);//
            this.btnNaranai.BackColor = Color.Yellow;
            this.btnNaranai.Visible = false;

            //----------
            // 向きボタン
            //----------
            this.btnMuki = new Shape_BtnBox("向き", 330, 590);

            //----------
            // 初期配置ボタン
            //----------
            this.btnSyokihaichi = new Shape_BtnBox("初期配置", 450, 590);

            //----------
            // クリアーボタン
            //----------
            this.btnClear = new Shape_BtnBox("クリアー", 450, 630);

            //----------
            // 再生ボタン
            //----------
            this.btnPlay = new Shape_BtnBox("再生", 710, 0);

            //----------
            // コマ送りボタン
            //----------
            this.btnForward = new Shape_BtnBox("コマ送り", 710, 40);

            //----------
            // 戻るボタン
            //----------
            this.btnBackward = new Shape_BtnBox("戻る", 710, 80);

            //----------
            // 将棋エンジン起動ボタン
            //----------
            this.btnShogiEngineKido = new Shape_BtnBox("将棋エンジン起動", 600, 120);

            //----------
            // 出力切替ボタン
            //----------
            this.btnSyuturyokuKirikae = new Shape_BtnBox("出力切替", 0, 730, 60, 23, 10.0f);

            //----------
            // [▲]～[打]符号ボタン
            //----------
            int ox = 30;
            int oy = 40;
            int w = 23;
            int h = 23;
            float fontSize = 13.0f;
            this.btnFugo_Sente = new Shape_BtnBox("▲", ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Gote = new Shape_BtnBox("△", ox, 1 * h + oy, w, h, fontSize);
            ox += w;

            this.btnFugo_1 = new Shape_BtnBox("１", 0 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_2 = new Shape_BtnBox("２", 1 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_3 = new Shape_BtnBox("３", 2 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_4 = new Shape_BtnBox("４", 0 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_5 = new Shape_BtnBox("５", 1 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_6 = new Shape_BtnBox("６", 2 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_7 = new Shape_BtnBox("７", 0 * w + ox, 2 * h + oy, w, h, fontSize);
            this.btnFugo_8 = new Shape_BtnBox("８", 1 * w + ox, 2 * h + oy, w, h, fontSize);
            this.btnFugo_9 = new Shape_BtnBox("９", 2 * w + ox, 2 * h + oy, w, h, fontSize);
            this.btnFugo_Dou = new Shape_BtnBox("同", 3 * w + ox, 0 * h + oy, w, h, fontSize);
            ox += (4 + 1) * w;

            this.btnFugo_Fu = new Shape_BtnBox("歩", 0 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Hisya = new Shape_BtnBox("飛", 1 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Kaku = new Shape_BtnBox("角", 2 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Kyo = new Shape_BtnBox("香", 3 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Kei = new Shape_BtnBox("桂", 4 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Gin = new Shape_BtnBox("銀", 5 * w + ox, 0 * h + oy, w, h, fontSize);

            this.btnFugo_Kin = new Shape_BtnBox("金", 0 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_Oh = new Shape_BtnBox("王", 1 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_Gyoku = new Shape_BtnBox("玉", 2 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_Tokin = new Shape_BtnBox("と", 3 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_Narikyo = new Shape_BtnBox("成香", 4 * w + ox, 1 * h + oy, 2 * w, h, fontSize);

            this.btnFugo_Narikei = new Shape_BtnBox("成桂", 0 * w + ox, 2 * h + oy, 2 * w, h, fontSize);
            this.btnFugo_Narigin = new Shape_BtnBox("成銀", 2 * w + ox, 2 * h + oy, 2 * w, h, fontSize);
            this.btnFugo_Ryu = new Shape_BtnBox("竜", 4 * w + ox, 2 * h + oy, w, h, fontSize);
            this.btnFugo_Uma = new Shape_BtnBox("馬", 5 * w + ox, 2 * h + oy, w, h, fontSize);
            ox += (6 + 1) * w;

            this.btnFugo_Yoru = new Shape_BtnBox("寄", 0 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Hiku = new Shape_BtnBox("引", 1 * w + ox, 0 * h + oy, w, h, fontSize);
            this.btnFugo_Agaru = new Shape_BtnBox("上", 2 * w + ox, 0 * h + oy, w, h, fontSize);

            this.btnFugo_Migi = new Shape_BtnBox("右", 0 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_Hidari = new Shape_BtnBox("左", 1 * w + ox, 1 * h + oy, w, h, fontSize);
            this.btnFugo_Sugu = new Shape_BtnBox("直", 2 * w + ox, 1 * h + oy, w, h, fontSize);

            this.btnFugo_Nari = new Shape_BtnBox("成", 0 * w + ox, 2 * h + oy, w, h, fontSize);
            this.btnFugo_Funari = new Shape_BtnBox("不成", 1 * w + ox, 2 * h + oy, 2 * w, h, fontSize);
            this.btnFugo_Da = new Shape_BtnBox("打", 3 * w + ox, 2 * h + oy, w, h, fontSize);
            this.btnFugo_Zenkesi = new Shape_BtnBox("全消", 5 * w + ox, 0 * h + oy, w, h, fontSize);

            this.btnFugo_KokokaraSaifu = new Shape_BtnBox("ここから採譜", 9 * w + ox, 0 * h + oy, w, h, fontSize);

        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 対局の描画の一式は、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Paint(
            object sender, PaintEventArgs e, Kifu_Document kifuD, ILoggerFileConf logTag
            )
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            //----------
            // 将棋盤
            //----------
            this.Shogiban.Paint(e.Graphics);

            //----------
            // 駒置き、駒袋
            //----------
            for (int i = 0; i < this.KomadaiArr.Length;i++ )
            {
                Shape_PnlKomadai k = this.KomadaiArr[i];
                k.Paint( e.Graphics);
            }

            //----------
            // 駒
            //----------
            foreach (Shape_BtnKoma koma in this.BtnKomaDoors)
            {
                koma.Paint(e.Graphics, this, kifuD, logTag);
            }

            //----------
            // 符号表示
            //----------
            this.lblFugo.Paint(e.Graphics);

            //----------
            // 先後表示
            //----------
            this.lblSengo.Text = GameTranslator.Sengo_ToKanji(kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)));
            this.lblSengo.Paint(e.Graphics);

            //----------
            // 成るボタン
            //----------
            this.BtnNaru.Paint(e.Graphics);

            //----------
            // 成らないボタン
            //----------
            this.BtnNaranai.Paint(e.Graphics);

            //----------
            // 向きボタン
            //----------
            this.BtnMuki.Paint(e.Graphics);

            //----------
            // 初期配置ボタン
            //----------
            this.BtnSyokihaichi.Paint(e.Graphics);

            //----------
            // クリアーボタン
            //----------
            this.BtnClear.Paint(e.Graphics);

            //----------
            // 再生ボタン
            //----------
            this.BtnPlay.Paint(e.Graphics);

            //----------
            // コマ送りボタン
            //----------
            this.BtnForward.Paint(e.Graphics);

            //----------
            // 戻るボタン
            //----------
            this.BtnBackward.Paint(e.Graphics);

            //----------
            // 将棋エンジン起動ボタン
            //----------
            this.BtnShogiEngineKido.Paint(e.Graphics);

            //----------
            // 出力切替ボタン
            //----------
            this.BtnSyuturyokuKirikae.Paint(e.Graphics);

            //----------
            // [▲]～[打]符号ボタン
            //----------
            this.BtnFugo_Sente.Paint(e.Graphics);
            this.BtnFugo_Gote.Paint(e.Graphics);
            this.BtnFugo_1.Paint(e.Graphics);
            this.BtnFugo_2.Paint(e.Graphics);
            this.BtnFugo_3.Paint(e.Graphics);
            this.BtnFugo_4.Paint(e.Graphics);
            this.BtnFugo_5.Paint(e.Graphics);
            this.BtnFugo_6.Paint(e.Graphics);
            this.BtnFugo_7.Paint(e.Graphics);
            this.BtnFugo_8.Paint(e.Graphics);
            this.BtnFugo_9.Paint(e.Graphics);
            this.BtnFugo_Dou.Paint(e.Graphics);
            this.BtnFugo_Fu.Paint(e.Graphics);
            this.BtnFugo_Hisya.Paint(e.Graphics);
            this.BtnFugo_Kaku.Paint(e.Graphics);
            this.BtnFugo_Kyo.Paint(e.Graphics);
            this.BtnFugo_Kei.Paint(e.Graphics);
            this.BtnFugo_Gin.Paint(e.Graphics);
            this.BtnFugo_Kin.Paint(e.Graphics);
            this.BtnFugo_Oh.Paint(e.Graphics);
            this.BtnFugo_Gyoku.Paint(e.Graphics);
            this.BtnFugo_Tokin.Paint(e.Graphics);
            this.BtnFugo_Narikyo.Paint(e.Graphics);
            this.BtnFugo_Narikei.Paint(e.Graphics);
            this.BtnFugo_Narigin.Paint(e.Graphics);
            this.BtnFugo_Ryu.Paint(e.Graphics);
            this.BtnFugo_Uma.Paint(e.Graphics);
            this.BtnFugo_Yoru.Paint(e.Graphics);
            this.BtnFugo_Hiku.Paint(e.Graphics);
            this.BtnFugo_Agaru.Paint(e.Graphics);
            this.BtnFugo_Migi.Paint(e.Graphics);
            this.BtnFugo_Hidari.Paint(e.Graphics);
            this.BtnFugo_Sugu.Paint(e.Graphics);
            this.BtnFugo_Nari.Paint(e.Graphics);
            this.BtnFugo_Funari.Paint(e.Graphics);
            this.BtnFugo_Da.Paint(e.Graphics);
            this.BtnFugo_Zenkesi.Paint(e.Graphics);
            this.BtnFugo_KokokaraSaifu.Paint(e.Graphics);


        gt_EndMethod:
            ;
        }


        /// <summary>
        /// 移動直後の駒を取得。
        /// </summary>
        /// <returns>移動直後の駒。なければヌル</returns>
        public Shape_BtnKoma Btn_MovedKoma()
        {
            Shape_BtnKoma btn = null;

            if (K40.Error != this.MovedKoma)
            {
                btn = this.BtnKomaDoors[(int)this.MovedKoma];
            }

            return btn;
        }

        /// <summary>
        /// つまんでいる駒。
        /// </summary>
        /// <returns>つまんでいる駒。なければヌル</returns>
        public Shape_BtnKoma Btn_TumandeiruKoma(Shape_PnlTaikyoku shape_PnlTaikyoku)
        {
            Shape_BtnKoma found = null;

            if (-1 != shape_PnlTaikyoku.HTumandeiruKoma)
            {
                found = this.BtnKomaDoors[shape_PnlTaikyoku.HTumandeiruKoma];
            }

            return found;
        }

    }


}
