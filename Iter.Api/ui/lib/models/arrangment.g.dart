// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'arrangment.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Arrangement _$ArrangementFromJson(Map<String, dynamic> json) => Arrangement(
      id: json['id'] as String,
      name: json['name'] as String,
      agency: Agency.fromJson(json['agency'] as Map<String, dynamic>),
      description: json['description'] as String,
      startDate: DateTime.parse(json['startDate'] as String),
      endDate: json['endDate'] == null
          ? null
          : DateTime.parse(json['endDate'] as String),
      prices: (json['prices'] as List<dynamic>)
          .map((e) => ArrangementPrice.fromJson(e as Map<String, dynamic>))
          .toList(),
      images: (json['images'] as List<dynamic>)
          .map((e) => ImageModel.fromJson(e as Map<String, dynamic>))
          .toList(),
      destinations: (json['destinations'] as List<dynamic>)
          .map((e) => Destination.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$ArrangementToJson(Arrangement instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'agency': instance.agency,
      'description': instance.description,
      'startDate': instance.startDate.toIso8601String(),
      'endDate': instance.endDate?.toIso8601String(),
      'prices': instance.prices,
      'images': instance.images,
      'destinations': instance.destinations,
    };
