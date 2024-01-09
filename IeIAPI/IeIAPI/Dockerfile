# Etapa intermedia para instalar dependencias
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS install-dependencies
WORKDIR /app

RUN apt-get update && apt-get install -y \
    wget \
    unzip \
    libgdiplus \
    libxss1 \
    libappindicator3-1 \
    libasound2 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libnss3 \
    libgdk-pixbuf2.0-0 \
    libgtk-3-0 \
    libx11-xcb1 \
    libxcb-dri3-0 \
    fonts-liberation \
    libgbm1 \
    libpango-1.0-0 \
    libpangocairo-1.0-0 \
    libxcursor1 \
    libxdamage1 \
    libxi6 \
    libxcomposite1 \
    libxrandr2 \
    libxss1 \
    libxtst6 \
    xdg-utils \
    libfreetype6 \
    libharfbuzz-icu0 \
    libfontconfig1 \
    libc6 \
    libglib2.0-0 \
    && rm -rf /var/lib/apt/lists/*

# Download and install Chrome
RUN apt-get update \
    && apt-get install -y gnupg \
    && wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor -o /usr/share/keyrings/google-chrome-archive-keyring.gpg \
    && echo "signed-by /usr/share/keyrings/google-chrome-archive-keyring.gpg" > /etc/apt/trusted.gpg.d/google-chrome-archive-keyring.gpg \
    && echo "deb [arch=amd64 signed-by=/usr/share/keyrings/google-chrome-archive-keyring.gpg] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list \
    && apt-get update \
    && apt-get install -y google-chrome-stable

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./IeIAPI.csproj .
RUN dotnet restore

COPY . .
RUN dotnet build -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Copiar los archivos publicados desde la fase de publicación
COPY --from=publish /app/publish .

# Copiar los archivos de la etapa de instalación
COPY --from=install-dependencies /usr/bin/google-chrome-stable /usr/bin/

# Definir el punto de entrada para la aplicación
EXPOSE 5000
EXPOSE 56581
CMD ["dotnet", "IeIAPI.dll"]