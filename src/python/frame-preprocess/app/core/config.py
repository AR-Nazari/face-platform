from pydantic import BaseSettings


class Settings(BaseSettings):
    app_name: str = "Face Platform - Frame Preprocess Service"
    debug: bool = False

    class Config:
        env_prefix = "FRAME_PREPROCESS_"


settings = Settings()
