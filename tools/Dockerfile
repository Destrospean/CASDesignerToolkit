FROM debian:bookworm
# Add contrib and non-free components to the sources file
RUN sed -i 's/main/main contrib non-free non-free-firmware/g' /etc/apt/sources.list.d/debian.sources
# Add 32-bit x86 architecture for Wine
RUN dpkg --add-architecture i386
# Update the package lists and install the needed packages
RUN apt update && apt install -y dirmngr ca-certificates gnupg && gpg --homedir /tmp --no-default-keyring --keyring gnupg-ring:/usr/share/keyrings/mono-official-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && chmod +r /usr/share/keyrings/mono-official-archive-keyring.gpg && echo "deb [signed-by=/usr/share/keyrings/mono-official-archive-keyring.gpg] https://download.mono-project.com/repo/debian stable-buster main" | tee /etc/apt/sources.list.d/mono-official-stable.list && apt update && apt install -y dos2unix mono-complete gtk-sharp2 monodevelop rar unrar wine32:i386 && rm -rf /var/lib/apt/lists/*
# Unpack older .NET frameworks for Mono
COPY tools/mono*.rar /tmp
RUN unrar x /tmp/mono-older-frameworks.part01.rar -y -op/usr/lib/ && rm /tmp/mono*.rar
