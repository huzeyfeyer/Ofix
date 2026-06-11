# Ofix

**Ofix** is een Belgische online autoplatform waar bezoekers tweedehands- en nieuwe voertuigen kunnen zoeken en bekijken. Advertenties worden uitsluitend door het Ofix-team beheerd via het admin-gedeelte — er is geen publieke “plaats advertentie”-flow.

Website: [ofix.be](https://ofix.be) · Kantoor: Excelsiorlaan 31, 1930 Zaventem

---

## Functies

### Publiek (bezoekers)

| Onderdeel | Route | Beschrijving |
|-----------|-------|--------------|
| Startpagina | `/` | Intro, merkenstrip, geïntegreerd zoekpaneel (merk, model, bouwjaar, prijs, carrosserie), recente advertenties, feature-blokken |
| Marktplaats | `/Marketplace` | Filteren, sorteren en doorbladeren van actieve advertenties |
| Advertentiedetail | `/Marketplace/Detail?id=…` | Foto-galerij (inclusief fullscreen), specs, prijs en beschrijving |
| Contact | `/Contact` | Adres, telefoon, e-mail, Google Maps |

- Tweetalige UI: **Nederlands (nl-BE)** en **Turks (tr)**
- Site-footer met snelle links, contactgegevens en social placeholders
- Groen gebruikers-thema (navbar/footer); geen advertentie-plaatsing voor gewone gebruikers

### Beheer (admin)

Toegang via permissies of de `admin`-rol. Donkerblauw admin-thema op alle pagina’s zolang je ingelogd bent als beheerder.

| Onderdeel | Route | Beschrijving |
|-----------|-------|--------------|
| Merken | `/Brands` | Automerken beheren (logo, volgorde, actief/inactief) |
| Modellen | `/Models` | Modellen per merk |
| Submodellen | `/SubModels` | Uitvoeringen / varianten |
| Advertenties | `/CarListings` | Voertuigen aanmaken, bewerken, foto’s uploaden, status |

Onder **Administratie** staan ook ABP-modules: Identity (gebruikers/rollen), instellingen, enz.

---

## Tech stack

| Laag | Technologie |
|------|-------------|
| Framework | [ABP Framework](https://abp.io/) — layered DDD template |
| Runtime | .NET 10 |
| UI | ASP.NET Core MVC / Razor Pages, Bootstrap 5, Font Awesome |
| Database | SQL Server (standaard: LocalDB) via Entity Framework Core |
| Auth | OpenIddict (ingebouwd in `Ofix.Web`) |
| Mapping | Mapperly |
| Lokalisatie | `Ofix.Domain.Shared/Localization/Ofix/` |

---

## Vereisten

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet)
- [Node.js v18 of v20](https://nodejs.org/) (voor ABP client-side libraries)
- SQL Server of [SQL Server LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (Windows)

---

## Snel starten

### 1. Client-side libraries installeren

```bash
cd c:\Projects\Ofix
abp install-libs
```

> Alleen nodig na clone of wanneer je nieuwe front-end packages toevoegt.

### 2. Database aanmaken en seeden

Pas indien nodig de connection string aan in:

- `src/Ofix.Web/appsettings.json`
- `src/Ofix.DbMigrator/appsettings.json`

Standaard:

```
Server=(LocalDb)\MSSQLLocalDB;Database=Ofix;Trusted_Connection=True;TrustServerCertificate=true
```

Voer daarna de migrator uit:

```bash
dotnet run --project src/Ofix.DbMigrator
```

Dit past migraties toe en seedt o.a. merken, modellen en de standaard admin-gebruiker.

### 3. Applicatie starten

```bash
dotnet run --project src/Ofix.Web
```

Open: **https://localhost:44352/**

### Standaard inloggegevens (ABP seed)

| Gebruiker | Wachtwoord |
|-----------|------------|
| `admin` | `1q2w3E*` |

Wijzig dit in productie.

---

## Projectstructuur

```
Ofix/
├── src/
│   ├── Ofix.Domain.Shared/      # Constanten, enums, lokalisatie
│   ├── Ofix.Domain/               # Entities, domain services, seed data
│   ├── Ofix.Application.Contracts/# DTO's, service-interfaces, permissies
│   ├── Ofix.Application/          # Application services
│   ├── Ofix.EntityFrameworkCore/  # DbContext, migraties, repositories
│   ├── Ofix.HttpApi/              # API-controllers (o.a. brand logo)
│   ├── Ofix.HttpApi.Client/       # Client proxies
│   ├── Ofix.Web/                  # Razor Pages UI, wwwroot, branding
│   └── Ofix.DbMigrator/           # Database migratie + seed console
└── test/                          # Unit- en integratietests
```

Solution-bestand: `Ofix.slnx`

### Belangrijkste domeinmodellen

- **Brand** — automerk
- **Model** — model binnen een merk
- **SubModel** — uitvoering/variant
- **CarListing** — advertentie (prijs, jaar, km, brandstof, transmissie, carrosserie, status)
- **CarListingImage** — foto’s bij een advertentie
- **Feature / FeatureCategory** — uitrusting en categorieën

> De map `Books/` is overblijfsel van het ABP-startersjabloon en wordt niet gebruikt in de publieke Ofix-flow.

### Belangrijke UI-locaties (`Ofix.Web`)

| Pad | Doel |
|-----|------|
| `Pages/Index/` | Startpagina en partials |
| `Pages/Marketplace/` | Marktplaats en detailpagina |
| `Pages/CarListings/` | Admin advertentiebeheer |
| `Pages/Brands/`, `Models/`, `SubModels/` | Admin catalogus |
| `Pages/Contact/` | Contactpagina |
| `Components/Layout/OfixSiteFooter/` | Site-footer |
| `Components/Layout/OfixBrandingHead/` | Favicon en logo-meta |
| `Layout/OfixLayoutHelper.cs` | Admin vs. gebruiker body-class |
| `wwwroot/global-styles.css` | Globale stijlen, navbar, footer, thema’s |

---

## Lokalisatie

Teksten staan in JSON-bestanden:

- `src/Ofix.Domain.Shared/Localization/Ofix/nl-BE.json` (primair)
- `src/Ofix.Domain.Shared/Localization/Ofix/tr.json`

Nieuwe UI-teksten altijd in beide bestanden toevoegen.

---

## Configuratie

| Instelling | Bestand | Opmerking |
|------------|---------|-----------|
| `ConnectionStrings:Default` | `appsettings.json` | SQL Server connection |
| `App:SelfUrl` | `Ofix.Web/appsettings.json` | Basis-URL (standaard `https://localhost:44352`) |
| `AuthServer` | `Ofix.Web/appsettings.json` | OpenIddict / authority |
| Geüploade foto’s | `wwwroot/uploads/car-listings/` | WebP-bestanden per advertentie |

### OpenIddict-certificaat (productie)

Voor productie is een signing certificate vereist (`openiddict.pfx` in `Ofix.Web`):

```bash
dotnet dev-certs https -v -ep openiddict.pfx -p 2c1c2a09-cee5-4f03-b05b-4b10cafca30d
```

Zie [ABP — Configuring OpenIddict](https://abp.io/docs/latest/Deployment/Configuring-OpenIddict) en [OpenIddict documentatie](https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html).

---

## Tests uitvoeren

```bash
dotnet test Ofix.slnx
```

Testprojecten: `Ofix.Domain.Tests`, `Ofix.Application.Tests`, `Ofix.EntityFrameworkCore.Tests`, `Ofix.Web.Tests`.

---

## Nieuwe database-migratie

```bash
cd src/Ofix.EntityFrameworkCore
dotnet ef migrations add <MigratieNaam> --startup-project ../Ofix.Web

cd ../Ofix.DbMigrator
dotnet run
```

---

## Deployment

Deployen volgt hetzelfde patroon als elke ASP.NET Core-app. Let op connection string, OpenIddict-certificaat en `wwwroot/uploads` voor persistente foto-opslag.

- [ABP Deployment](https://abp.io/docs/latest/Deployment/Index)
- [ABP Layered Application Template](https://abp.io/docs/latest/solution-templates/layered-web-application)

---

## ABP-documentatie

- [Web Application Development Tutorial](https://abp.io/docs/latest/tutorials/book-store/part-1)
- [Domain Driven Design in ABP](https://abp.io/docs/latest/framework/architecture/domain-driven-design)
