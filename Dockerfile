FROM mono:4.2.3.4

RUN curl -sL https://deb.nodesource.com/setup_5.x | bash - \
    && apt-get install -y nodejs

WORKDIR /app

COPY . /app

RUN './build.sh'

EXPOSE 8083

CMD ["mono", "/app/build/CafeApp.Web.exe"]