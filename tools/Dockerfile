FROM debian:bookworm

# Add contrib and non-free components to the sources.list file
RUN sed -i 's/main/main contrib non-free non-free-firmware/g' /etc/apt/sources.list.d/debian.sources

# Add 32-bit x86 architecture for Wine
RUN dpkg --add-architecture i386

# Update the package lists and install the needed packages
RUN apt update && apt install -y mono-complete gtk-sharp2 rar unrar wine32:i386 && rm -rf /var/lib/apt/lists/*

# Unpack older .NET frameworks for Mono
#COPY libs/mono*.rar /tmp && unrar x /tmp/mono-older-frameworks.part01.rar -y -op/usr/lib/ && rm /tmp/mono*.rar

CMD ["/bin/bash", "/CASDesignerToolkit/tools/bundle_debian.sh"]
