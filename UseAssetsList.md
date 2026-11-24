# 使用素材一覧（UseAssetsList）

『ギルドマスター・ダンジョン（仮）』で使用する外部素材の一覧・ライセンス・配置パスをまとめたドキュメントです。

- **最終更新日**: 2026-11-24
- **対象プロジェクト**: `UnityProject`（Unity 6.7 / 3D URP）

---

## 1. Kenney 素材（新規導入）

すべて [Kenney](https://kenney.nl/assets) 配布の **CC0 1.0 Universal（パブリックドメイン）** 素材です。商用・非商用を問わず利用・改変可能で、クレジット表記は必須ではありません。
Unity での重複インポートや肥大化を避けるため、各パックから Unity 向けの最適形式（3D は FBX＋テクスチャ、UI・アイコンは 2x/Double PNG、パーティクルは透過 PNG、音声は OGG）を抽出して `UnityProject/Assets/ThirdParty/Kenney/` 配下へ配置し、各フォルダに `License.txt` を同梱しています。

### 3D モデル（盤面・ダンジョン・ギルド背景）

| パック名 | Ver. | 配布元 | ライセンス | 配置先（`UnityProject/` 基点） | 抽出内容 | 主な用途 |
|---|---|---|---|---|---|---|
| **Mini Dungeon** | 2.0 | https://kenney.nl/assets/mini-dungeon | CC0 1.0 | `Assets/ThirdParty/Kenney/MiniDungeon/` | FBX モデル、Textures（`colormap.png`, `variation-a.png`） | 立体すごろく盤面の床マス・階段・宝箱・柱・扉・小物 |
| **Fantasy Town Kit** | 2.0 | https://kenney.nl/assets/fantasy-town-kit | CC0 1.0 | `Assets/ThirdParty/Kenney/FantasyTownKit/` | FBX モデル、Textures（`colormap.png`, `variation-a.png`） | ギルド館・地上入口・周辺ジオラマ・建築小物 |

### UI・ボードゲームパーツ・アイコン

| パック名 | Ver. | 配布元 | ライセンス | 配置先（`UnityProject/` 基点） | 抽出内容 | 主な用途 |
|---|---|---|---|---|---|---|
| **UI Pack (RPG Expansion)** | 1.0 | https://kenney.nl/assets/ui-pack-rpg-expansion | CC0 1.0 | `Assets/ThirdParty/Kenney/UIPackRPG/` | `PNG/` 一式（パネル・ボタン・HP/MPバー・矢印） | 木枠＆羊皮紙テイストの共通 UI・ゲージ・ボタン |
| **Fantasy UI Borders** | 1.0 | https://kenney.nl/assets/fantasy-ui-borders | CC0 1.0 | `Assets/ThirdParty/Kenney/FantasyUIBorders/` | `PNG/Double/` 一式（1080p向け高解像度・9-Slice用枠/仕切り） | バトル窓・PTカード（選択中金枠含む）・パネル装飾枠 |
| **Board Game Pack** | 1.0 | https://kenney.nl/assets/boardgame-pack | CC0 1.0 | `Assets/ThirdParty/Kenney/BoardgamePack/` | `PNG/`（カード・チップ・ダイス・駒）＋ `Audio/`（元 `Bonus/` のダイス/カード/チップSE） | テーブル周囲のボードゲーム小物・敵カード枠・盤面操作 SE |
| **Board Game Icons** | 1.1 | https://kenney.nl/assets/board-game-icons | CC0 1.0 | `Assets/ThirdParty/Kenney/BoardGameIcons/` | `Icons/`（Double 128px PNG） | 探索方針・ターン・イベント・ボードゲーム記号アイコン |
| **Game Icons** | 1.0 | https://kenney.nl/assets/game-icons | CC0 1.0 | `Assets/ThirdParty/Kenney/GameIcons/` | `Icons/`（White 2x PNG・Unity UI の Color で着色可） | 一時停止／再生速度（1x/2x/3x）・設定・システム系アイコン |

### 2D スプライト・エフェクト・サウンド

| パック名 | Ver. | 配布元 | ライセンス | 配置先（`UnityProject/` 基点） | 抽出内容 | 主な用途 |
|---|---|---|---|---|---|---|
| **Tiny Dungeon** | 1.0 | https://kenney.nl/assets/tiny-dungeon | CC0 1.0 | `Assets/ThirdParty/Kenney/TinyDungeon/` | `Tiles/` 個別PNG、`Tilemap/tilemap_packed.png` | プロトタイプ用 2D 敵キャラ・アイテム・小アイコン仮素材 |
| **Particle Pack** | 1.0 | https://kenney.nl/assets/particle-pack | CC0 1.0 | `Assets/ThirdParty/Kenney/ParticlePack/` | `Textures/`（透過 PNG 80種・重複回転版を除外） | HD-2D 風の舞う光の粒子、攻撃・魔法・回復エフェクト |
| **RPG Audio** | 1.0 | https://kenney.nl/assets/rpg-audio | CC0 1.0 | `Assets/ThirdParty/Kenney/RPGAudio/` | `Audio/`（OGG 50種） | 足音・扉・宝箱・装備・戦闘・本をめくる音などの RPG SE |
| **Interface Sounds** | 1.0 | https://kenney.nl/assets/interface-sounds | CC0 1.0 | `Assets/ThirdParty/Kenney/InterfaceSounds/` | `Audio/`（OGG 100種） | ボタンクリック・方針切替・確認・キャンセル等の UI SE |

---

## 2. 既存導入済み素材

| パック名 | 配布元 / 備考 | ライセンス | 配置先（`UnityProject/` 基点） | 収録内容 | 主な用途 |
|---|---|---|---|---|---|
| **2D Medieval Fantasy Character Pack** | プロジェクト既存導入アセット（Unity Asset Store 等） | 配布元ライセンスに準拠 | `Assets/2D Medieval Fantasy Character Pack/` | 男女別ファンタジー職業キャラ PNG 30種（Fighter / Knight / Clergyman / Magician / Robber / Archer 等）、`PSD.zip`、`ExampleScenes` | 初期6人PT「創設の夜明け」および雇用冒険者・NPC の 2D 駒／バトル／UI 立ち絵 |

---

## 3. 配置・インポート時の補足メモ

- **3D モデルのカラーバリエーション**: `MiniDungeon/Textures/` および `FantasyTownKit/Textures/` には `colormap.png` に加え `variation-a.png` も配置済みです。URP Lit マテリアルの Base Map を差し替えることで、階層やダンジョン種別ごとの色変えに活用できます。
- **UI テクスチャ設定**: `UIPackRPG` / `FantasyUIBorders` / `BoardGameIcons` / `GameIcons` / `BoardgamePack/PNG` の画像は、Unity 側で `Texture Type = Sprite (2D and UI)` に設定して使用します（`FantasyUIBorders` およびパネル素材は Sprite Editor で 9-Slice 設定推奨）。
- **ドット絵（Tiny Dungeon）**: `Filter Mode = Point (no filter)`、`Compression = None` に設定すると滲まず表示できます。
- **Game Icons（White 2x）**: 白単色アイコンのみを配置しているため、`Image.color` でクリーム・茶・金・黒など UI テーマ色へ自在に着色できます。
