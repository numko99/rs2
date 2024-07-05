// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_names.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserNamesResponse _$UserNamesResponseFromJson(Map<String, dynamic> json) =>
    UserNamesResponse(
      id: json['id'] as String,
      firstName: json['firstName'] as String,
      lastName: json['lastName'] as String,
      agencyName: json['agencyName'] as String?,
    );

Map<String, dynamic> _$UserNamesResponseToJson(UserNamesResponse instance) =>
    <String, dynamic>{
      'id': instance.id,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'agencyName': instance.agencyName,
    };
