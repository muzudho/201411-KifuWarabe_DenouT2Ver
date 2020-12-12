# Kifuwarabe_DenouT2

2020年11月の 電竜戦から きふわらね(Kifuwarane)にリネームして開発再開だぜ☆（＾〜＾）  

## Dev and Config

|               | ファイル                                                                                     |
| ------------- | -------------------------------------------------------------------------------------------- |
| ソース        | `Kifuwarabe_DenouT2/Kifuwarane.sln`                                            |
| 将棋エンジン ランタイム | Kifuwarabe_DenouT2/Builds/Release/Grayscale.Kifuwarane.Engine.exe |
| 設定ファイル1 | `Kifuwarabe_DenouT2/Biulds/Release/Grayscale.Kifuwarane.Engine.exe.config` |
| 設定ファイル2 | `Kifuwarabe_DenouT2/Profile/Engine.toml`                                                     |

* `Kifuwarabe_DenouT2` のトップ・ディレクトリーに `Logs` ディレクトリーを作成してください。
* `Kifuwarane.sln` を `Release` モードで ビルドしてください。
* 設定ファイル1 の中にある `Profile` のパスを、 設定ファイル2 の親ディレクトリー `Profile` に合わせてください。  

## Manual

`第２回 将棋電王トーナメント 出場版` の復刻版。  
(2020-11-14) usinewgame 時に ログファイルを強制削除するコードをハードコーディングした。  

```
v(^▽^)vｲｪｰｲ☆　説明するぜ☆ｗｗｗｗｗ


　　第２回　将棋電王トーナメント

　　　　　　きふわらべ　２４　位　☆！

　　　　　　　　　（２５ソフト参加中）
　　　　　　　　　http://ex.nicovideo.jp/denou/tournament2014/


　　きふわらべ　は　フリーで公開するが、
　　１１月１６日（日）の出展イベント　デジゲー博　にも　２０１４年作品集として
　　持って行く（予定）なんだぜ☆　（他の何かと一緒　数百円予定）

　　http://digigame-expo.org/ 　D-19b ぐれーすけーる（むずでょ）
    


────────────────────────────────────────
[製　品　名]　　将棋ＧＵＩ　　きふならべ
　　　　　　　　将棋エンジン　きふわらべ
　　　　　　　　　（第２回　将棋電王トーナメント用　現地お持ち帰りセット）
[バージョン]　　Ver 1.10.0 リタイアバージョン(※注1)
　　　　　　　　　　KifuLarabe Ver 1.02.2
　　　　　　　　　　KifuWarabe Ver 1.02.3
　　　　　　　　　　KifuNarabe Ver 1.09.0改
[作　成　日]　　2014/11/01
[著　作　者]　　TAKAHASHI Satoshi
　　　　　　　　　　（H.N.むずでょ）
[ホームページ]　
　　　　　　　　旧サークル：　http://xenon.wiki.fc2.com/　（※サークル活動停止）
　　　　　　　　現サークル：　http://grayscale.iza-yoi.net/　（※活動中）
[必要なもの]    .NET Framework 4.5 以降
        http://www.microsoft.com/ja-jp/download/details.aspx?id=17851
[動作　環境]    Windows7 で確認。

　※注1 　リタイアバージョン　……　開催日までに　将棋のルール通りに指せるものが
　　　　　　　　　　　　　　　　　　作れていないし、思考エンジンに未着手であった
　　　　　　　　　　　　　　　　　　ことから、
　　　　　　　　　　　　　　　　　　第２回将棋電王トーナメント　ルール
　　　　　　　　　　　　　　　　　　第４章第１１条にそって、最初の対戦の開始後
　　　　　　　　　　　　　　　　　　棄権を申し出ようと決め、
　　　　　　　　　　　　　　　　　　初手で飛車・角・香が駒を飛び越して反則負けを
　　　　　　　　　　　　　　　　　　するという不具合だけは封じ込めるために　駒は
　　　　　　　　　　　　　　　　　　１マスしか動けない、という拘束を施したもの。
　　　　　　　　　　　　　　　　　　“初手で反則手負けするよりは　王手回避漏れ
　　　　　　　　　　　　　　　　　　　するまで１手でも長く駒を動かそう”
　　　　　　　　　　　　　　　　　　と思った改造☆
　　　　　　　　　　　　　　　　　　これにより４手で投了する攻茶花電さんにだけは
　　　　　　　　　　　　　　　　　　勝てた☆ｗｗｗ
　　　　　　　　　　　　　　　　　　
　　　　　　　　　　　　　　　　　　大会前日に棄権の旨をメールすると
　　　　　　　　　　　　　　　　　　運営から電話がかかってきたんだが、
　　　　　　　　　　　　　　　　　　将棋エンジンを作っている人を応援したいとの
　　　　　　　　　　　　　　　　　　ことで、出ていいことが分かったので
　　　　　　　　　　　　　　　　　　結局　全局　８対局（不戦勝１含む）出た☆ｗｗ
────────────────────────────────────────


ソフトの概要
============

　　　　コンピューター将棋用の将棋エンジンと、
　　　　将棋ＧＵＩだぜ☆


起動方法
========

    【きふわらべ】の起動方法

        別途、将棋所（しょうぎどころ）というソフトを入手してください。
        http://www.geocities.jp/shogidokoro/

        将棋所を使って、将棋エンジンを動かしてください。
        将棋所の使い方は、将棋所の使い方を読んでください☆



        きふわらべの将棋エンジンは、

　　　　VS_きふわらべセット - 電王戦終了コピー/Xenon.KifuWarabe.exe
　　　　　　　　　　　　　　　　　　　　　　　~~~~~~~~~~~~~~~~~~~~~
        似た名前が多いですが、 “Ｗ” が目印です。ＷａｒａｂｅのＷ。


    【きふならべ】の起動方法

        C#ソースコード/KifuNarabe/KifuNarabe/bin/Release/Xenon.KifuNarabe.exe

        をダブルクリックしてください。ＧＵＩが起動します。
        
        設定ファイルは、同じフォルダーの　settei.xml　です。
        将棋エンジンを起動するためには、settei.xml ファイルで
        ファイルパスを設定してください。






        なんだか　ゴミファイル　がいっぱい　入っているようなんだが、
        大会に持っていったまま、全部　入れておくぜ☆ｗｗ

        ファイル名の頭に # が付いているファイルは　わたしは
        消していいファイルの目印に付けてあるんで、消して構わないぜ☆ｗｗｗ
        むしろ　消さないと　どんどん増えてＰＣを圧迫するものもあるんだぜ☆ｗｗ



作者への連絡先
==============

    muzudho1@gmail.com (変更しました 2014-02-21)

    グーグルドキュメント　ツリーエディター要望／不具合表（一般公開）
    https://docs.google.com/spreadsheet/ccc?key=0AkvZtqfBiwxfdEZIUHBFLXpEZE1sX2trQ2Vnd2FJb0E&usp=sharing
    元々は他のアプリ用ですが、ここで全てのアプリを承っています。


取り扱い種別
============

    フリーソフト
    二条項ＢＳＤライセンス

            『将棋ＧＵＩ  きふならべ』、
            『将棋ＧＵＩ　きふわらべ』は、二条項ＢＳＤライセンスで配布します。
                                          ~~~~~~~~~~~~~~~~~~~~~~

            意訳しますと、ＢＳＤライセンスが言っているのは次の２つのことです。

                ・著作者は、どんな損害も  保障してくれません。（無保障）
                ・著作者は、どんな責任も  負ってくれません。  （免責）

            二条項とは、何が２つなのかと言いますと、
            配る人と  もらう人の間に、約束が次の２つあるということです。

                （１つ目）変更の有無を問わず、ソースコードの配布・利用を許可するときの条件

                            �@著作権表示
                            �A二条項（*1）
                            �B無保証、免責の条項（*1）
                            
                            の３つを保持すること。

                （２つ目）変更の有無を問わず、バイナリー形式のファイルの配布・利用を許可するときの条件

                            「�@著作権表示
                              �A二条項（*1）
                              �B無保証、免責の条項（*1）」
                            は、ドキュメント、または他の資料で配布すること。

                *1 …… 原文の二条項ＢＳＤライセンスをテキストファイルを
                        添付しているので、
                        再配布するときは、これを忘れず一緒に配布してください。


動作環境
========

    Windows7 で動作確認済み。


インストール・アンインストール方法
==================================

    [インストール方法]

        圧縮ファイルを解凍するだけ。

    [アンインストール方法]

        フォルダー丸ごと削除するだけ。


使用しているライブラリ
======================

    Xenon.KifuLarabe.dll    ライブラリです。

    Xenon.KifuWarabe.exe    USIプロトコル対応の将棋エンジンです。

    Xenon.KifuNarabe.exe    将棋ＧＵＩです。


使用している素材
================

    特になし


--------------------------------------------------------------------------------
更新履歴
--------------------------------------------------------------------------------

KifuNarabe Ver1.09.0改 (2014-11-04)

        ・第２回将棋電王トーナメント　出場バージョンを
          持って帰ってきたまま公開☆

        [既知のバグ]
          駒は１マスずつしか動かない設定にしてあるし、
          王さまは王手回避漏れするし、
          思考エンジンは作られていないんだぜ☆


KifuNarabe Ver1.09.0 (2014-10-01)

        ・リーガルムーブの実装のなかばです。


KifuNarabe Ver1.08.0 (2014-08-31)

    [修正した不具合]

        ・相手の陣地から出たときに、成れなかった不具合を修正しました。
        ・王や金が相手陣地に入ったときに、成る／成らない　ダイアログボックスが
          出ていた不具合を修正しました。
        ・駒台の上の駒を　取ってしまう　不具合を修正しました。
        ・２つあった出力欄を１つにし、[出力切替]ボタンを付けました。
        ・「コンピューター最善手ボタン」「HTMLボタン」は削除しました。


KifuNarabe Ver1.07.0 (2014-08-10)

    将棋エンジンとやり取りができるように、ソースコードの改造を進めています。

    [既知の不具合]

          「将棋エンジン起動ボタン」、「コンピューター最善手ボタン」ともに
          作りかけです。


        ・[将棋エンジン起動]ボタンは、
          Xenon.KifuNarabe.exe と同じフォルダーにある shogiEngine.exe が起動します。
          コンピューターが後手です。

          通信タイミングがあっておらず、
          ２手目から  すぐに  挙動、棋譜がおかしくなります。
          今後、変更予定です。


        ・[コンピューターの最善手]ボタンは、駒が１つ上に移動するだけです。
          今後、変更予定です。


KifuNarabe Ver1.06.0 (2014-08-01)

    （１）  [戻る]ボタンを付けました。
    （２）  出力欄の上段では「▲同飛（歩取り）」のように、取った駒も
            出力されるようになりました。
            このまま入力欄に入れることができます。丸括弧は無視されます。

    [既知の不具合]
        現在、王・玉　の区別なく、「王」のみになっています。

        「寄」「引」「上」「左」「右」「直」「成」「不成」「打」
        の対応は終えていますが、間違っているところもあるかもしれません。

        HTMLボタンは作者用です。

        出力欄の２つ目は、SFEN形式を出していますが、
        合っているのか間違っているのか分かりません。



KifuNarabe Ver1.05.0 (2014-07-27)

    SFEN形式への対応を進めました。

    [ここから採譜]ボタンを押すと、押した局面を初期盤面とする棋譜を出力します。

    入力欄は１つにしました。
    「▲７六歩」と「position startpos moves 」のどちらにも使えます。

    [既知の不具合]
        現在、王・玉　の区別なく、「王」のみになっています。

        「寄」「引」「上」「左」「右」「直」「成」「不成」「打」
        の対応は終えていますが、間違っているところもあるかもしれません。

        HTMLボタンは作者用です。

        出力欄の２つ目は、SFEN形式を出していますが、
        合っているのか間違っているのか分かりません。

KifuNarabe Ver1.04.0 (2014-07-25)

    符号の「同」「打」への対応を終えました。

    [既知の不具合]
        現在、王・玉　の区別なく、「王」のみになっています。

        「寄」「引」「上」「左」「右」「直」「成」「不成」「打」
        の対応は終えていますが、間違っているところもあるかもしれません。

        HTMLボタンは作者用です。

        入力欄の２つ目は開発中です。
        出力欄の２つ目は、SFEN形式を出していますが、間違っているかもしれません。

KifuNarabe Ver1.03.0 (2014-07-22)

    [既知の不具合]
        現在、王・玉　の区別なく、「王」のみになっています。

        「寄」「引」「上」「左」「右」「直」「成」「不成」「打」
        への対応を　更に　進めましたが、間違っているところもあるかもしれません。
                    ~~~~
        HTMLボタンは作者用です。

        出力欄が２つに増えていますが、下のは作りかけです。


KifuNarabe Ver1.02.0 (2014-06-22)

    棋譜符号入力ボタンを付けました。

    [既知の不具合]
        現在、王・玉　の区別なく、「王」のみになっています。
        
        「寄」「引」「上」「左」「右」「直」「成」「不成」「打」
        への対応を進めましたが、間違っているところもあるかもしれません。


KifuNarabe Ver1.01.0 (2014-06-22)

    棋譜の符号の読込み、表示。
    [既知の不具合]
        金直や、銀右など、「寄」「金」「銀」「直」には未対応です。


KifuNarabe Ver1.00.0 (2014-06-18)

    公開


以上
```
