# Aoyo.Taiga

**Aoyo.Taiga** is a lightweight C# application that listens for webhooks from [Taiga](https://taiga.io/) and sends a notification to a specified Discord channel via a bot.

## ✨ Features

- Receives webhook events from Taiga.
- Sends formatted messages to a designated Discord channel.
- Lightweight and easy to run (locally or in a container).
- Supports configuration via `environment variables` or `appsettings.json`.

## ⚙️ Required Configuration

The app requires the following configuration keys to function:

### Environment variables (`ENV`)
- `Discord__Token` – your Discord bot token.
- `Discord__Id` – target Discord channel ID.
- `Taiga__Key` – secret key to validate incoming Taiga webhooks.

### OR `appsettings.json`:

```json
{
  "Discord": {
    "Token": "your_discord_token",
    "Id": "your_channel_id"
  },
  "Taiga": {
    "Key": "your_taiga_webhook_secret"
  }
}
```
Only one method of configuration is required. You can choose between env vars or appsettings.json.
🐳 Run with Docker

You can pull and run the prebuilt image from GitHub Container Registry (GHCR):
```bash
docker run -d \
  -e Discord__Token=your_token \
  -e Discord__Id=your_channel_id \
  -e Taiga__Key=your_webhook_key \
  -p 8080:80 \
  ghcr.io/yawaflua/aoyo.taiga:latest
```

🧪 Run Locally

Make sure you have the .NET SDK installed.
1. Clone the repo
```shell
git clone https://github.com/yawaflua/Aoyo.Taiga.git
```
cd Aoyo.Taiga

2. Configure credentials

Either set env variables or fill in appsettings.json.
3. Run the app

```bash
dotnet run --project Aoyo.Taiga
```

By default, it will listen on http://localhost:8080/aoyo.
🚀 Behavior

Once a webhook is sent from Taiga (with the correct key), the app will verify it and forward a message to the Discord channel defined by the Discord__Id.
📫 Webhook Endpoint

You can point your Taiga webhook to:

`POST https://example.mycooldomain.co.il/aoyo/api/v1/TaigaWebHook/post`

Make sure to include the Taiga__Key as a query parameter or in the header to pass validation.

Project can be used under [Apache 2.0](LICENCE)

