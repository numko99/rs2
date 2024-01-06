import 'package:json_annotation/json_annotation.dart';

import 'agency.dart';

part 'arrangment.g.dart';

@JsonSerializable()
class Arrangement {
  final String id;
  final String agencyId;
  final String agency;
  final String name;
  final String description;
  final double price;
  final int capacity;
  final DateTime startDate;
  final DateTime endDate;
  final DateTime dateCreated;
  final DateTime dateModified;

  Arrangement({
    required this.id,
    required this.agencyId,
    required this.agency,
    required this.name,
    required this.description,
    required this.price,
    required this.capacity,
    required this.startDate,
    required this.endDate,
    required this.dateCreated,
    required this.dateModified,
  });

  factory Arrangement.fromJson(Map<String, dynamic> json) => _$ArrangementFromJson(json);

  Map<String, dynamic> toJson() => _$ArrangementToJson(this);
}
