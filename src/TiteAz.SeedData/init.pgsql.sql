CREATE TABLE events
(
    id bigserial NOT NULL PRIMARY KEY,
    category varchar(500) NOT NULL,
    streamid uuid NOT NULL,
    transactionid uuid NOT NULL,
    metadata text NOT NULL,
    bodytype varchar(500) NOT NULL,
    body text NOT NULL,
    who uuid NOT NULL,
    _when timestamp NOT NULL,
    version int NOT NULL,
    appversion varchar(20) NOT NULL
);

create table read_model (
    id bigserial primary key,
    stream_id uuid not null,
    type varchar(500) not null,
    body text not null,
    last_update timestamp not null
);