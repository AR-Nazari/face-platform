# Message Queue Format – Skeleton

Defines RabbitMQ message formats for:

- IP camera frames
- Async face processing
- Group reload commands

Each message will include:

- `message_type`
- `tenant_id`, `group_id`
- for frames: image reference or raw bytes
- for reload: group id and version

(جزئیات بعد از طراحی کامل فلو دوربین و batch پر می‌شود.)
