// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'arrangement_price.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ArrangementPrice _$ArrangementPriceFromJson(Map<String, dynamic> json) =>
    ArrangementPrice(
      accommodationType: json['accommodationType'] as String?,
      price: (json['price'] as num?)?.toDouble(),
    );

Map<String, dynamic> _$ArrangementPriceToJson(ArrangementPrice instance) =>
    <String, dynamic>{
      'accommodationType': instance.accommodationType,
      'price': instance.price,
    };
