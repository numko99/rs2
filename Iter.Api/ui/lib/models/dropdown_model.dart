import 'package:json_annotation/json_annotation.dart';

part 'dropdown_model.g.dart';

@JsonSerializable()

class DropdownModel {
  int id;
  String name;

  DropdownModel({required this.id, required this.name});

  factory DropdownModel.fromJson(Map<String, dynamic> json) => _$DropdownModelFromJson(json);

  Map<String, dynamic> toJson() => _$DropdownModelToJson(this);

}

