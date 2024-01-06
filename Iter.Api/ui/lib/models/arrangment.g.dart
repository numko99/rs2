// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'arrangment.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Arrangement _$ArrangementFromJson(Map<String, dynamic> json) => Arrangement(
      id: json['id'] as String,
      agencyId: json['agencyId'] as String,
      agency: json['agency'] as String,
      name: json['name'] as String,
      description: json['description'] as String,
      price: (json['price'] as num).toDouble(),
      capacity: json['capacity'] as int,
      startDate: DateTime.parse(json['startDate'] as String),
      endDate: DateTime.parse(json['endDate'] as String),
      dateCreated: DateTime.parse(json['dateCreated'] as String),
      dateModified: DateTime.parse(json['dateModified'] as String),
    );

Map<String, dynamic> _$ArrangementToJson(Arrangement instance) =>
    <String, dynamic>{
      'id': instance.id,
      'agencyId': instance.agencyId,
      'agency': instance.agency,
      'name': instance.name,
      'description': instance.description,
      'price': instance.price,
      'capacity': instance.capacity,
      'startDate': instance.startDate.toIso8601String(),
      'endDate': instance.endDate.toIso8601String(),
      'dateCreated': instance.dateCreated.toIso8601String(),
      'dateModified': instance.dateModified.toIso8601String(),
    };
