# Space Invaders

Klassisen Space Invaders -pelin 2D-toteutus Unity-pelimoottorilla.

## Tietoa projektista

Projekti on rakennettu Unity-versiolla `6000.6.8f1` käyttäen Universal Render Pipeline (URP) 2D -putkea ja Unityn uutta Input System -pakettia.

## Pelin ominaisuudet

Pelaaja ohjaa ruudun alareunassa liikkuvaa alusta ja pyrkii tuhoamaan ylhäältä lähestyvän vihollismuodostelman.

- Vihollismuodostelma koostuu viidestä rivistä ja yhdeksästä sarakkeesta (`EnemyTop` 30 pistettä, `EnemyMid` 20 pistettä, `EnemyBottom` 10 pistettä).
- Muodostelma liikkuu vaakasuunnassa, laskeutuu alaspäin osuessaan reunaan (`edgeLimit`) ja kiihtyy jokaisen tuhotun vihollisen myötä. Viimeisen jäljellä olevan vihollisen nopeus kasvaa edelleen.
- Viholliset ampuvat satunnaisesti alaspäin, ja pelikentällä on tuhoutuvia suojaesteitä (`BunkerBlock`), jotka murenevat ammusten aiheuttamasta aluevauriosta (`SplashHit`).
- Pelaajalla on kolme elämää. Osumasta käynnistyy tuhoutumis- ja uudelleensyntymisanimaatio.
- Kaikkien vihollisten tuhoaminen käynnistää uuden aallon. Peli päättyy tappioon, jos elämät loppuvat tai viholliset saavuttavat alarajan tai osuvat suojaesteisiin.

Vihollisten nopeuskertoimiin on lisäksi kätketty pieniä matemaattisiin vakioihin, kuten piihin ja tauhun, viittaavia pääsiäismunia.

## Ohjaus

- Peli käynnistetään aloitusruudusta välilyönnillä (`Space`).
- Aluksen liikuttaminen vasemmalle ja oikealle: `A` / `D` tai vasen / oikea nuolinäppäin.
- Ampuminen: `Space`.

## Projektin rakenne

Tärkeimmät pelikohtaiset kansiot ja tiedostot hakemistossa `Assets/`:

- `Assets/Scenes/SampleScene.unity`: Pelin pääkohtaus ja käyttöliittymäelementit.
- `Assets/Scripts/GameManager.cs`: Pelin tilanhallinta, pistelasku, elämät sekä alku- ja loppuvalikot.
- `Assets/Scripts/PlayerController.cs`: Pelaajan liikkuminen, ampuminen, tuhoutumisefekti ja uudelleensyntyminen.
- `Assets/Scripts/PlayerBullet.cs`: Pelaajan ammuksen liike ja törmäystunnistus.
- `Assets/Scripts/EnemyFormation.cs`: Vihollisaaltojen luonti, muodostelman liike, nopeuden skaalaus ja vihollisten ampuminen.
- `Assets/Scripts/Enemy.cs`: Yksittäisen vihollisen pisteytys ja osumakäsittely.
- `Assets/Scripts/EnemyBullet.cs`: Vihollisen ammuksen liike ja törmäystunnistus.
- `Assets/Scripts/BunkerBlock.cs`: Tuhoutuvien suojaesteiden vauriomalli ja aluevaurio.
- `Assets/Prefabs/`: Vihollisten, ammusten ja suojaesteen prefabit.
- `Assets/Sprites/`: Pelissä käytettävät 2D-spritet.
- `Assets/Settings/Build Profiles/Windows.asset`: Windows-käännöksen build-profiili.

## Kehitysympäristö ja käynnistys

Vaatimukset:
- Unity `6000.6.8f1`
- Käytetyt pääpaketit (`Packages/manifest.json`): `com.unity.render-pipelines.universal` (`17.6.0`), `com.unity.inputsystem` (`1.20.1`), `com.unity.ugui` (`2.6.0`).

Käyttöönotto:
1. Kloonaa repositorio.
2. Avaa projektikansio Unity Hubin kautta Unity-editorissa (`6000.6.8f1`).
3. Avaa kohtaus `Assets/Scenes/SampleScene.unity` ja käynnistä peli editorin Play-painikkeella.
4. Windows-koontiversion voi kääntää Unityn Build Profiles -näkymästä käyttäen profiilia `Assets/Settings/Build Profiles/Windows.asset`.
