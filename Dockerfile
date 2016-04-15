FROM mono:4.2.3.4

RUN curl -sL https://deb.nodesource.com/setup_5.x | bash - \
    && apt-get install -y nodejs

COPY client /app/client

WORKDIR /app/client

RUN npm install

COPY .paket /app/.paket

COPY paket.lock paket.dependencies build.sh build.fsx /app/

RUN mono /app/.paket/paket.bootstrapper.exe \
    && mono /app/.paket/paket.exe install

COPY src /app/src

WORKDIR /app

RUN './build.sh'

EXPOSE 8083

CMD ["mono", "/app/build/CafeApp.Web.exe"]