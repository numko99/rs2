// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'arrangement_search_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ArrangementSearchResponse _$ArrangementSearchResponseFromJson(
        Map<String, dynamic> json) =>
    ArrangementSearchResponse(
      id: json['id'] as String,
      name: json['name'] as String,
      agencyName: json['agencyName'] as String,
      agencyRating: (json['agencyRating'] as num).toDouble(),
      startDate: DateTime.parse(json['startDate'] as String),
      endDate: json['endDate'] == null
          ? null
          : DateTime.parse(json['endDate'] as String),
      arrangementStatusId: json['arrangementStatusId'] as int?,
      arrangementStatusName: json['arrangementStatusName'] as String?,
      mainImage: json['mainImage'] == null
          ? null
          : ImageModel.fromJson(json['mainImage'] as Map<String, dynamic>),
      isReserved: json['isReserved'] as bool?,
      rating: (json['rating'] as num?)?.toDouble(),
    );

Map<String, dynamic> _$ArrangementSearchResponseToJson(
        ArrangementSearchResponse instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'agencyName': instance.agencyName,
      'agencyRating': instance.agencyRating,
      'startDate': instance.startDate.toIso8601String(),
      'endDate': instance.endDate?.toIso8601String(),
      'arrangementStatusId': instance.arrangementStatusId,
      'arrangementStatusName': instance.arrangementStatusName,
      'mainImage': instance.mainImage,
      'isReserved': instance.isReserved,
      'rating': instance.rating,
    };
