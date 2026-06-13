#!/usr/bin/env bash

generateDtos() {
  nswag openapi2csclient \
          /input:"$1" \
          /output:"$2" \
          /namespace:BitcoinMarketLoader.Application.Dtos.CnbApi \
          /GenerateDtoTypes:true \
          /GenerateClientClasses:false \
          /JsonLibrary:SystemTextJson
          /GenerateOptionalPropertiesAsNullable:true  
}

generateDtos cnbapi.json CnbApiDtos.cs
