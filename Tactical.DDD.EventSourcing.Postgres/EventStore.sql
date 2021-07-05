CREATE TABLE IF NOT EXISTS events
(
    id bigserial NOT NULL,
    stream_id uuid NOT NULL, 
    stream_version int NOT NULL,
    stream_name text NOT NULL,
    data jsonb NOT NULL,
    meta jsonb,
    created_on timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    PRIMARY KEY (id),
    UNIQUE (stream_id, stream_version)
);
