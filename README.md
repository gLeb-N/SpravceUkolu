# Dokumentace k závěrečnému projektu: Správce úkolů

## 1. Zadání projektu
Aplikace **Správce úkolů** je konzolový program napsaný v jazyce C#, který slouží k efektivní organizaci a evidenci každodenních povinností. Uživatel může úkoly flexibilně přidávat, prohlížet si jejich seznam s barevně odlišenou prioritou a označovat je za splněné. 

Hlavním cílem aplikace je poskytnout přehledné uživatelské rozhraní a zajistit, aby se rozpracovaná data neztrácela po vypnutí programu. Proto aplikace automaticky ukládá a načítá data z externího souboru a při ukončení čistí databázi od již hotových úkolů, čímž udržuje seznam stále aktuální.

---

## 2. Model tříd a jejich vazby
Aplikace je navržena podle principů objektově orientovaného programování (OOP) a je rozdělena do čtyř logických celků (tříd):

* **`Program`**: Zajišťuje uživatelské rozhraní (UI), komunikaci s uživatelem přes konzoli, zobrazování menu a zpracování vstupů. Využívá instanci třídy `SpravceUkoluLogika`.
* **`Ukol`**: Reprezentuje samotnou datovou entitu (objekt úkolu). Drží vlastnosti jako název, priorita a stav splnění.
* **`SpravceUkoluLogika`**: Obsahuje hlavní aplikační logiku. Spravuje dynamickou kolekci (`List<Ukol>`) a volá metody pro uložení/načtení dat.
* **`SpravceSouboru`**: Statická třída, která má na starosti výhradně technickou práci s diskem – zápis JSON textu do souboru a jeho zpětné čtení.

### Vazby mezi třídami:
1.  **Asociace (Agregace)**: Třída `SpravceUkoluLogika` v sobě drží seznam objektů `List<Ukol>`. Třída `Ukol` existuje samostatně, ale logika s ní pracuje uvnitř dynamické kolekce.
2.  **Závislost (Dependency)**: Třída `Program` přímo závisí na `SpravceUkoluLogika`, protože přes ni ovládá všechna data. Třída `SpravceUkoluLogika` zase závisí na statických metodách třídy `SpravceSouboru` při ukládání a načítání.

---

## 3. Struktura aplikace (Třídy a metody)

### Třída `Program`
Základní spouštěcí třída aplikace obsahující uživatelské menu.
* `Main(string[] args)`: Vstupní bod programu. Inicializuje načtení dat, spouští hlavní `while` cyklus menu a reaguje na volby uživatele (1–4).
* `ZobrazHlavicku()`: Pomocná metoda pro vykreslení jednotného grafického záhlaví aplikace v azurové barvě.
* `PridatUkol()`: Zpracovává textový vstup od uživatele pro název úkolu (kontroluje prázdné znaky), volá načtení priority a vytváří nový objekt `Ukol`.
* `NactiPrioritu()`: Zobrazuje podmenu pro výběr priority (Vysoká, Střední, Nízká) a validuje správnost zadaného čísla.
* `ZobrazitUkoly()`: Vykresluje seznam všech úkolů. U nesplněných úkolů barevně odlišuje text priority (Červená = Vysoká, Žlutá = Střední, Zelená = Nízká), splněné úkoly zobrazuje šedivě.
* `SplnitUkol()`: Vypíše úkoly s indexy, načte volbu uživatele, ověří, zda jde o platné číslo, a předá index ke zpracování logice.
* `CekejNaStisk()`: Pozastaví program (`Console.ReadKey`) a čeká na reakci uživatele před návratem do hlavního menu.

### Třída `Ukol`
Datový model pro jeden úkol.
* **Vlastnosti (Properties)**:
    * `Nazev` (string): Název úkolu.
    * `Priorita` (string): Úroveň důležitosti (Vysoká/Střední/Nízká).
    * `JeSplneno` (bool): Indikátor, zda je úkol hotový (defaultně `false`).
* **Konstruktor**: `Ukol(string nazev, string priorita)` – Nastaví název, prioritu a nastaví stav `JeSplneno` na nesplněno.

### Třída `SpravceUkoluLogika`
Komponenta spravující vnitřní stav databáze úkolů.
* `_ukoly` (private List<Ukol>): Soukromá dynamická kolekce uchovávající aktuální úkoly v paměti.
* `PridatUkol(Ukol ukol)`: Přidá nový objekt úkolu do listu `_ukoly`.
* `ZiskatVsechnyUkoly()`: Vrací celou kolekci `List<Ukol>` pro potřeby zobrazení v UI.
* `SplnitUkol(int index)`: Ověří, zda zadaný index existuje v rozmezí kolekce, a pokud ano, změní vlastnost `JeSplneno` daného úkolu na `true`.
* `VymazatSplneneUkoly()`: Profylaktická metoda, která profiltruje seznam, ponechá pouze úkoly se stavem `JeSplneno == false` a starou kolekci jimi nahradí. Volá se před uložením při ukončení programu.
* `UlozitDoSouboru()`: Předává aktuální list úkolů datové vrstvě `SpravceSouboru`.
* `NacistZeSouboru()`: Volá datovou vrstvu a přepisuje vnitřní list `_ukoly` načtenými daty.

### Třída `SpravceSouboru`
Statická třída zajišťující perzistenci (trvalé ukládání) dat.
* `_cestaKSouboru` (private static string): Obsahuje název souboru (`ukoly.json`), do kterého se data ukládají.
* `UlozitData(List<Ukol> seznamUkoly)`: Serializuje (převede) objekt C# kolekce na textový řetězec formátu JSON pomocí `JsonSerializer.Serialize` a zapíše ho na disk přes `File.WriteAllText`.
* `NacistData()`: Zkontroluje existenci souboru pomocí `File.Exists`. Pokud neexistuje, vrátí nový prázdný `List<Ukol>`. Pokud existuje, přečte text přes `File.ReadAllText` a deserializuje jej zpět na objekty C# pomocí `JsonSerializer.Deserialize`.

---

## 4. Popis práce se soubory
Aplikace využívá pro ukládání dat moderní strukturovaný formát **JSON** (JavaScript Object Notation), což zajišťuje skvělou čitelnost jak pro stroj, tak pro člověka. Práce se soubory je plně automatizovaná:
1.  **Při spuštění programu**: V metodě `Main` se okamžitě volá metoda `NacistZeSouboru()`. Program se podívá do složky, kde je spuštěn. Pokud soubor `ukoly.json` chybí (např. při prvním spuštění), aplikace nespadne, ale tiše vytvoří prázdnou databázi. V opačném případě data načte a obnoví předchozí stav.
2.  **Při ukončení programu (Volba 4)**: Než se program zavře, spustí se čistící proces `VymazatSplneneUkoly()`, který odstraní dokončené úkoly. Následně se zavolá `UlozitDoSouboru()`, která aktuální nesplněné úkoly převede na JSON text a přepíše soubor `ukoly.json`. Data jsou tak bezpečně uložena pro příští spuštění.

---

## 5. Popis ovládání (Uživatelská příručka)
Program se kompletně ovládá v textovém režimu konzole pomocí klávesnice. Po spuštění se zobrazí hlavní menu s nabídkou možností:

* **Volba 1 – Zobrazit všechny úkoly**: Vypíše očíslovaný seznam všech aktuálních úkolů. Uživatel hned vidí stav úkolu (ikona `[ ]` pro nesplněný, `[X]` pro splněný) a jeho důležitost, která je pro rychlou orientaci barevně podbarvená.
* **Volba 2 – Přidat nový úkol**: Program vyzve uživatele k zadání názvu úkolu. Pokud uživatel zadá prázdný text, program ho na chybu upozorní a výzvu opakuje. Následně se otevře podmenu, kde uživatel stiskem čísla `1`, `2` nebo `3` vybere prioritu úkolu.
* **Volba 3 – Označit úkol za splněný**: Zobrazí se očíslovaný seznam úkolů. Uživatel zadá číslo (index) úkolu, který dokončil. Program validuje vstup – pokud uživatel zadá text místo čísla nebo zadá číslo úkolu, které neexistuje, aplikace vypíše červené chybové hlášení a vrátí se zpět.
* **Volba 4 – Ukončit program**: Provede automatické pročištění seznamu (smaže hotové věci), uloží zbývající úkoly do souboru `ukoly.json` a bezpečně aplikaci zavře.
