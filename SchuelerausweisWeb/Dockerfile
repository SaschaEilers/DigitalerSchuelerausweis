FROM php:8.1-apache
RUN apt-get update && apt-get install -y --no-install-recommends libldap2-dev libpng-dev libfreetype6-dev
RUN docker-php-ext-configure gd --with-freetype
RUN docker-php-ext-configure ldap --with-libdir=lib/x86_64-linux-gnu
RUN docker-php-ext-install -j$(nproc) ldap gd
RUN mv "$PHP_INI_DIR/php.ini-production" "$PHP_INI_DIR/php.ini"
COPY . /var/www/html